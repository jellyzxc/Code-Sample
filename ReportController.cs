using eDRCNet.IntraCountyEconomy.Model.DF;
using eDRCNet.IntraCountyEconomy.WebSite.Areas.DF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDRCNet.IntraCountyEconomy.WebBLL.IDFProvier;
using System.IO;
using eDRCNet.IntraCountyEconomy.WebBLL.IFSProvier;
using System.Configuration;
using System.Web.Script.Serialization;
using eDRCNet.IntraCountyEconomy.WebBLL;
using eDRCNet.IntraCountyEconomy.WebBLL.Common;
using eDRCNet.IntraCountyEconomy.WebBLL.ISYSProvier;
using eDRCNet.IntraCountyEconomy.Model.SYS;

namespace eDRCNet.IntraCountyEconomy.WebSite.Areas.DF.Controllers
{
     
    public class ReportController : BaseController
    {
        //
        // GET: /DF/Report/
        /// <summary>
        /// 初始化页面，获取全部评价报告信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       //[OutputCache(Duration = 60, VaryByParam = "*")]
      [SysFilter(IsLogin = false)]
        public ActionResult ReportList(int chnid, int leafid) 
        {
            leafid = leafid > 10000000 ? (leafid - 10000000) : leafid;

            int pageIndex = 1;
            int pageSize = 25;
            ViewBag.RootId = leafid;

            if (Request["pageindex"] != null && Request["pageindex"] != "")
            {
                pageIndex = Convert.ToInt32(Request["pageindex"]);      //当前页码
            }
            if (Request["pagesize"] != null && Request["pagesize"] != "")
            {
                pageSize = Convert.ToInt32(Request["pagesize"]);       //页容量
            }
            ReportInfoViewModel model = new ReportInfoViewModel();
            ChnLeafInfo chLeaf = new ChnLeafInfo() { LeafId = leafid };
            model.LeafInfo = chLeaf;
            List<ReportBasicInfo> reports;
            int totalNum = 10;
            using (DFClient client = new DFClient())
            {
                List<ReportTagInfo> tagList = client.GetAllReportTagList(chLeaf.LeafId);
                List<ReportBasicInfo> TOPreports = client.GetTopCensorReportListByLeafId(chLeaf.LeafId);
                reports = client.GetCensorReportListByLeafId(out totalNum, chLeaf.LeafId, pageIndex, pageSize - TOPreports.Count, new Dictionary<string, string>(), "DelivedDate", false);
                ViewBag.Total = totalNum;
                List<ReportBasicInfo> newList = new List<ReportBasicInfo>();
                newList.AddRange(TOPreports);
                newList.AddRange(reports);
                model.BasicInfos = newList;
                model.TagInfos = tagList;


                this.ViewBag.NameP = client.GetChannelInfo(chnid).ChnName;
                this.ViewBag.NameC = client.GetLeafInfo(chnid,leafid).ChnName;

                client.Close();
            }


            return View(model);
        }
        /// <summary>
        /// 筛选条件
        /// </summary>
        /// <returns>字典对象</returns>
        protected Dictionary<string, string> GetCondition()
        {
            Dictionary<string, string> condition = new Dictionary<string, string>();

            if (Request["subject"] != null && !string.IsNullOrEmpty(Request["subject"]))
            {
                var subject = Request["subject"].Trim().Replace("'", "");       //项目名称
                condition.Add("Subject", subject);
            }
            if (Request["author"] != null && !string.IsNullOrEmpty(Request["author"]))
            {
                var author = Request["author"].Trim().Replace("'", "");         //作者
                condition.Add("Author", author);
            }
            if (Request["source"] != null && !string.IsNullOrEmpty(Request["source"]))
            {
                var source = Request["source"].Trim().Replace("'", "");         //来源
                condition.Add("Source", source);
            }
            if (Request["keywords"] != null && !string.IsNullOrEmpty(Request["keywords"]))
            {
                var keywords = Request["keywords"].Trim().Replace("'", "");     //关键字
                condition.Add("Keywords", keywords);
            }
            if (Request["censor"] != null && !string.IsNullOrEmpty(Request["censor"]))
            {
                var censor = Request["censor"].Trim().Replace("'", "");         //发布
                condition.Add("Censor", censor);
            }
            return condition;
        }
        /// <summary>
        /// 通过筛选获取评价报告信息
        /// </summary>
        /// <param name="id">叶子ID</param>
        /// <returns>json串</returns>
        public ActionResult GetReportListByFilters(int id)
        {
            int pageIndex = 1;
            int pageSize = 10;
            if (Request["pageindex"] != null && Request["pageindex"] != "")
            {
                pageIndex = Convert.ToInt32(Request["pageindex"]);             //当前页码
            }
            if (Request["pagesize"] != null && Request["pagesize"] != "")
            {
                pageSize = Convert.ToInt32(Request["pagesize"]);               //页容量
            }
            var condition = GetCondition();
            ReportInfoViewModel model = new ReportInfoViewModel();
            ChnLeafInfo chLeaf = new ChnLeafInfo() { LeafId = id };
            model.LeafInfo = chLeaf;
            List<ReportBasicInfo> reports;
            int totalNum = 10;
            using (DFClient client = new DFClient())
            {
                reports = client.GetCensorReportListByLeafId(out totalNum, chLeaf.LeafId, pageIndex, pageSize, condition, "", true);
                model.Total = totalNum;
                model.PageIndex = pageIndex;
                List<ReportBasicInfo> newList = new List<ReportBasicInfo>();
                //通过判断将置顶数据优先显示
                var ttop = (from r in reports where (r.IsTop == true && DateTime.Now >= (r.TopStartDate) && DateTime.Now <= (r.TopEndDate)) select r).ToList();
                var tnottop = (from r in reports where (r.IsTop == false || (r.IsTop == true && (DateTime.Now < (r.TopStartDate) || DateTime.Now > (r.TopEndDate)))) select r).ToList();
                newList.AddRange(ttop);
                newList.AddRange(tnottop);
                model.BasicInfos = newList;
                client.Close();
            }
            return Json(model,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取评价报告详细信息
        /// </summary>
        /// <param name="id">leafid</param>
        /// <param name="docid">docid</param>
        /// <returns>视图模型</returns>
       [DocFilter]   
        public ActionResult GetReportDetails(int id, long docid)
        {
            
            var model = new ReportInfoViewModel();
            ReportDetailInfo detail = new ReportDetailInfo();
            List<ReportFileInfo> fileInfoList = new List<ReportFileInfo>();
            using (DFClient client = new DFClient())
            {
                detail = client.GetReportDetailInfo(docid, id);
                fileInfoList = client.GetReportFileList(docid);
                //获取详细信息
                client.Close();
            }
            if (detail == null || fileInfoList == null)
            {
                return Redirect("/home/error");
            }
            model.DetailInfo = detail;
            model.FileInfos = fileInfoList;
            ViewBag.Title = detail.Subject;
            return View(model);
        }
        /// <summary>
        /// 通过标签名称获取贫家 报告列表
        /// </summary>
        /// <param name="id">leafid</param>
        /// <returns>对象</returns>
        public ActionResult GetReportListByTagName(int id)
        {
            string tagName = "";
            int pageIndex = 1;
            int pageSize = 10;
            if (Request["tagname"]!="" && !string.IsNullOrEmpty(Request["tagname"]))
            {
                tagName = Request["tagname"];
            }         
            ViewBag.RootId = id;
            if (Request["pageindex"] != null && Request["pageindex"] != "")
            {
                pageIndex = Convert.ToInt32(Request["pageindex"]);      //当前页码
            }
            if (Request["pagesize"] != null && Request["pagesize"] != "")
            {
                pageSize = Convert.ToInt32(Request["pagesize"]);       //页容量
            }
            ReportInfoViewModel model = new ReportInfoViewModel();
            ChnLeafInfo chLeaf = new ChnLeafInfo() { LeafId = id };
            model.LeafInfo = chLeaf;
            List<ReportBasicInfo> reports;
            int totalNum = 10;
            using (DFClient client = new DFClient())
            {
                if(tagName=="" && string.IsNullOrEmpty(tagName))
                {
                    reports = client.GetCensorReportListByLeafId(out totalNum, chLeaf.LeafId, pageIndex, pageSize, new Dictionary<string, string>(), "DelivedDate", false);
                }
                else if (tagName == "其他")
                {
                    reports = client.GetCensorReportListByLeafIdWithOutTagName(out totalNum, chLeaf.LeafId, pageIndex, pageSize, new Dictionary<string, string>(), "DelivedDate", false);
                }
                else
                {
                    reports = client.GetCensorReportListByLeafIdandTagName(out totalNum, chLeaf.LeafId, tagName, pageIndex, pageSize, new Dictionary<string, string>(), "DelivedDate", false);
                }
                model.Total = totalNum;
                model.PageIndex = pageIndex;
                List<ReportBasicInfo> newList = new List<ReportBasicInfo>();
                //通过判断将置顶数据优先显示
                var ttop = (from r in reports where (r.IsTop == true && DateTime.Now >= (r.TopStartDate) && DateTime.Now <= (r.TopEndDate)) select r).ToList();
                var tnottop = (from r in reports where (r.IsTop == false || (r.IsTop == true && (DateTime.Now < (r.TopStartDate) || DateTime.Now > (r.TopEndDate)))) select r).ToList();
                newList.AddRange(ttop);
                newList.AddRange(tnottop);
                model.BasicInfos = newList;               
                client.Close();
            }
            return Json(model);
        }
        /// <summary>
        /// 获取留言列表信息
        /// </summary>
        /// <returns>json串</returns>
        public ActionResult GetContentListByFilters()
        {
            long docid = 0; ;
            int pageIndex = 1;
            int pageSize = 5;
            int total = 0;
            if (Request["pageindex"] != null && Request["pageindex"] != "")
            {
                pageIndex = Convert.ToInt32(Request["pageindex"]);             //当前页码
            }
            if (Request["pagesize"] != null && Request["pagesize"] != "")
            {
                pageSize = Convert.ToInt32(Request["pagesize"]);               //页容量
            }
            if (Request["docid"] != null || !string.IsNullOrEmpty(Request["docid"]))
            {
                long.TryParse((Request["docid"]),out docid);
            }
            List<DocCommentInfo> infoList = new List<DocCommentInfo>();
            using (DFClient client = new DFClient())
            {
                infoList = client.GetCommentListByDocId(out total, docid, pageIndex, pageSize);
            }
            DocCommentViewModel docModel = new DocCommentViewModel() 
            {
                Data = infoList,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total
            };
            return Content(new JavaScriptSerializer().Serialize(docModel));
        }
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <returns></returns>
      
        public ActionResult AddCotent()
        {
           //获取登录用户信息
            UserLogin userLogin = new UserLogin(this.HttpContext);     //将当前页面信息传入UserLogin
            int userId = userLogin.GetUserId();                        //如果对象不为空，获取UserId
            string username="";
            if (userId == 0)
            {
                return null;
            }
           else{
                using (SYSClient client = new SYSClient())
                {
                    UserBasicInfo info = client.GetUserBaseInfo(userId);
                    username = info.UserName;
                    client.Close();
                }
            }
           //发表评论
            long docid = 0;
            bool isanonymous = false;
            string content = "";
            int result = -1;
            long parentid = 0;    //默认
            if (Request["isanonymous"] != null && Request["isanonymous"] != "")
            {
                isanonymous = Convert.ToBoolean(Request["isanonymous"]);             //当前页码
            }
            if (Request["content"] != null && Request["content"] != "")
            {
                content = Request["content"];               //页容量
            }
            if (Request["docid"] != null || !string.IsNullOrEmpty(Request["docid"]))
            {
                long.TryParse((Request["docid"]), out docid);
            }
            using (DFClient client = new DFClient())
            {
                result = client.InsertComment(docid, isanonymous, userId, username, content, parentid);
                client.Close();
            }
            return result > 0 ? Json(new { Status = "success", Msg = "发布成功" }) : Json(new { Status = "success", Msg = "发布失败" });
        }
    }
}
