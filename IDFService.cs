
using System.Data;
using eDRCNet.Framework;
using eDRCNet.IntraCountyEconomy.BLL.DF;
using eDRCNet.IntraCountyEconomy.BLL.SYS;
using eDRCNet.IntraCountyEconomy.Model;
using eDRCNet.IntraCountyEconomy.Model.DF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using eDRCNet.EF.DAL;
namespace eDRCNet.IntraCountyEconomy.WCFService.DF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class IDFService : IDF
    {
        #region DFDocComment 评论相关
        public List<DocCommentInfo> GetCommentListByChnId(int chnId, int curPage, int pagesize, out int total)
        {
            return SqlMapDAL.CreateNameQuery("GetCommentListByChnId")
           .SetParameter("ChnId", chnId)
           .ListEntityByPage<DocCommentInfo>(curPage, pagesize, out total);

        }
        public List<DocCommentInfo> GetCommentListByDocId(long DocId, int curPage, int pagesize, out int total)
        {
            return  SqlMapDAL.CreateNameQuery("GetCommentListByDocId")
           .SetParameter("DocId", DocId)
           .ListEntityByPage<DocCommentInfo>(curPage, pagesize, out total);
            
        }
        public List<DocCommentInfo> GetCommentListByUserId(int UserId, int curPage, int pagesize, out int total)
        {
           return SqlMapDAL.CreateNameQuery("GetCommentListByUserId")
          .SetParameter("UserId", UserId)
          .ListEntityByPage<DocCommentInfo>(curPage, pagesize, out total);
        }

        public List<DocCommentInfo> GetRelevantCommentListByUserId(int UserId, int curPage, int pagesize, out int total)
        {
            return SqlMapDAL.CreateNameQuery("GetRelevantCommentListByUserId")
           .SetParameter("UserId", UserId)
           .ListEntityByPage<DocCommentInfo>(curPage, pagesize, out total);
        }
        public List<DocCommentInfo> GetCommentListByContent(string Content, int curPage, int pagesize, out int total)
        {
            return SqlMapDAL.CreateNameQuery("GetCommentListByContent")
           .SetParameter("Content", Content)
           .ListEntityByPage<DocCommentInfo>(curPage, pagesize, out total);
        }
        public int InsertComment(long DocId, bool IsAnonymous, int UserId, string UserName, string Content, long ParentId)
        {
            string tt = SqlMapDAL.CreateNameQuery("InsertComment")
           .SetParameter("DocId", DocId)
           .SetParameter("IsAnonymous", IsAnonymous)
           .SetParameter("UserId", UserId)
           .SetParameter("UserName", UserName)
           .SetParameter("Content", Content)
           .SetParameter("ParentId", ParentId).ExecuteScalar().ToString();

            return Convert.ToInt32(tt);
        }
        public void  DelCommentByCommentId(long CommentId)
        {
            SqlMapDAL.CreateNameQuery("DelCommentByCommentId", true)
           .SetParameter("CommentId", CommentId).ExecuteNonQuery();
        }
        public DocCommentInfo GetDocCommentInfo(long CommentId)
        {
            return SqlMapDAL.CreateNameQuery("GetDocCommentInfoByCommentId")
            .SetParameter("CommentId", CommentId).Entity<DocCommentInfo>();
        }


        #endregion

        #region Report 评价报告

        public List<ReportBasicInfo> GetReportListByLeafId(int leafId, int curPage, int pagesize, Dictionary<string,string> condition,string tagName, string orderbyField, bool isAsc, out int total)
        {
            string sqlCondition = "";
            if (condition.Count > 0)
                sqlCondition = condition.Aggregate(sqlCondition, (current, kvp) => current + (" and [" + kvp.Key + "] like '%" + kvp.Value + "%' "));

            string orderby = "order by DelivedDate desc";
            if (!string.IsNullOrEmpty(orderbyField))
            {
                if (orderbyField.ToLower() == "createdate")
                    orderbyField = "a.createdate ";
                orderby = " order by " + orderbyField + " " + (isAsc ? "asc" : "desc");
            }

            total = 0;
            List<ReportBasicInfo> list;
            if (tagName == "")
                list = SqlMapDAL.CreateNameQuery("GetReportListByLeafId")
                                .AppendText(sqlCondition)
                                .AppendText(orderby)
                                .SetParameter("LeafId", leafId)
                                .ListEntityByPage<ReportBasicInfo>(curPage, pagesize, out total);
            else
                list = SqlMapDAL.CreateNameQuery("GetLeafReportListByTagId")
                                .AppendText(sqlCondition)
                                .AppendText(orderby)
                                .SetParameter("LeafId", leafId)
                                .SetParameter("TagName", tagName)
                                .ListEntityByPage<ReportBasicInfo>(curPage, pagesize, out total);
            return list;
        }
        //add jelly
        public List<ReportBasicInfo> GetTopCensorReportListByLeafId(int leafId) {
            List<ReportBasicInfo> list = SqlMapDAL.CreateNameQuery("GetTopCensorReportListByLeafId").SetParameter("LeafId", leafId).ListEntity<ReportBasicInfo>();
            return list;
        }
        public List<ReportBasicInfo> GetCensorReportListByLeafId(int leafId, int curPage, int pagesize, Dictionary<string, string> condition,  string orderbyField, bool isAsc, out int total)
        {
            string sqlCondition = "  ";
            if (condition.Count > 0)
           // sqlCondition = condition.Aggregate(sqlCondition, (current, kvp) => current + (" and [" + kvp.Key + "] like '%" + kvp.Value + "%' "));
            {
                sqlCondition += "and ( ";
                string[] where =condition.FirstOrDefault().Value.Split(' ');// 
                foreach (string ii in where) {

                    if (ii.Trim().Length > 0) {
                        sqlCondition += "  subject  like  '%" + ii.Trim() + "%'  and";
                    }
                }
                sqlCondition  = sqlCondition.Substring(0, sqlCondition.Length - 3);
                sqlCondition += " ) ";
            }
         
            string orderby = "order by DelivedDate desc";
            if (!string.IsNullOrEmpty(orderbyField))
            {
                if (orderbyField.ToLower() == "createdate")
                    orderbyField = "a.createdate ";
                orderby = " order by " + orderbyField + " " + (isAsc ? "asc" : "desc");
            }

            total = 0;
            List<ReportBasicInfo> list=SqlMapDAL.CreateNameQuery("GetCensorReportListByLeafId")
                                 .AppendText(sqlCondition)
                                 .AppendText(orderby)
                                .SetParameter("LeafId", leafId).ListEntityByPage<ReportBasicInfo>(curPage, pagesize, out total);
              
            return list;
        }
        //add jelly
        public List<ReportBasicInfo> GetCensorReportListByLeafIdandTagName(int leafId,string TagName, int curPage, int pagesize, Dictionary<string, string> condition, string orderbyField, bool isAsc, out int total)
        {
            string sqlCondition = "";
            if (condition.Count > 0)
                sqlCondition = condition.Aggregate(sqlCondition, (current, kvp) => current + (" and [" + kvp.Key + "] like '%" + kvp.Value + "%' "));

            string orderby = "order by DelivedDate desc";
            if (!string.IsNullOrEmpty(orderbyField))
            {
                if (orderbyField.ToLower() == "createdate")
                    orderbyField = "a.createdate ";
                orderby = " order by " + orderbyField + " " + (isAsc ? "asc" : "desc");
            }

            total = 0;
            List<ReportBasicInfo> list = SqlMapDAL.CreateNameQuery("GetCensorReportListByLeafIdandTagName")
                                .AppendText(sqlCondition)
                                .AppendText(orderby)
                                .SetParameter("LeafId", leafId).SetParameter("TagName", TagName).ListEntityByPage<ReportBasicInfo>(curPage, pagesize, out total);

            return list;
        }

        public List<ReportBasicInfo> GetCensorReportListByLeafIdWithOutTagName(int leafId, int curPage, int pagesize, Dictionary<string, string> condition, string orderbyField, bool isAsc, out int total)
        {
            string sqlCondition = "";
            if (condition.Count > 0)
                sqlCondition = condition.Aggregate(sqlCondition, (current, kvp) => current + (" and [" + kvp.Key + "] like '%" + kvp.Value + "%' "));

            string orderby = "order by DelivedDate desc";
            if (!string.IsNullOrEmpty(orderbyField))
            {
                if (orderbyField.ToLower() == "createdate")
                    orderbyField = "a.createdate ";
                orderby = " order by " + orderbyField + " " + (isAsc ? "asc" : "desc");
            }

            total = 0;
            List<ReportBasicInfo> list = SqlMapDAL.CreateNameQuery("GetCensorReportListByLeafIdWithOutTagName")
                                .AppendText(sqlCondition)
                                .AppendText(orderby)
                                .SetParameter("LeafId", leafId).ListEntityByPage<ReportBasicInfo>(curPage, pagesize, out total);

            return list;
        }

        public List<ReportBasicInfo> GetReportListByChnIdForHome(int chnId, int num)
        {
            string sqlCondition = "";
            List<ReportBasicInfo> list=new List<ReportBasicInfo> ();
            list = SqlMapDAL.CreateNameQuery("GetReportListByChnIdForHome")
                                .AppendText(sqlCondition)
                                .SetParameter("ChnId", chnId)
                                .SetParameter("Num", num).ListEntity<ReportBasicInfo>();
            return list;
        }



        public List<ReportBasicInfo> GetReportListByChnId(int chnId, int curPage, int pagesize, Dictionary<string, string> condition, string tagName, string orderbyField, bool isAsc, out int total)
        {
            string sqlCondition = "";
            if (condition.Count > 0)
                sqlCondition = condition.Aggregate(sqlCondition, (current, kvp) => current + (" and [" + kvp.Key + "] like '%" + kvp.Value + "%' "));

            string orderby = "order by DelivedDate desc";
            if (!string.IsNullOrEmpty(orderbyField))
            {
                if (orderbyField.ToLower() == "createdate")
                    orderbyField = "a.createdate ";
                orderby = " order by " + orderbyField + " " + (isAsc ? "asc" : "desc");
            }

            total = 0;
            List<ReportBasicInfo> list;
            if (tagName =="")
                list = SqlMapDAL.CreateNameQuery("GetReportListByChnId")
                                .AppendText(sqlCondition)
                                .AppendText(orderby)
                                .SetParameter("ChnId", chnId)
                                .ListEntityByPage<ReportBasicInfo>(curPage, pagesize, out total);
            else
                list = SqlMapDAL.CreateNameQuery("GetChannelReportListByTagId")
                                .AppendText(sqlCondition)
                                .AppendText(orderby)
                                .SetParameter("ChnId", chnId)
                                .SetParameter("TagName", tagName)
                                .ListEntityByPage<ReportBasicInfo>(curPage, pagesize, out total);
            return list;
        }

        public string InsertOrUpdateReportInfo(ReportDetailInfo info)
        {
            return (string)SqlMapDAL.CreateNameQuery("InsertOrUpdateReportInfo").SetParameter(info).ExecuteScalar();
        }

        public void DelReport(int leafId, string docIds)
        {
            SqlMapDAL.CreateNameQuery("DelReport")
                .ReplaceText("{0}",docIds)
                .SetParameter("LeafId", leafId)
                .ExecuteNonQuery();
        }
        public int UpdateReportContent(long docId, string content,int pageNo)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("UpdateReportContent")
                    .SetParameter("Content", content)
                    .SetParameter("DocId", docId)
                    .SetParameter("PageNo",pageNo)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        public int UpdateReportForCensor(int leafId, string docIds, bool censor, int censorUserId)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("UpdateReportForCensor")
                    .ReplaceText("{0}", docIds)
                    .SetParameter("LeafId", leafId)
                    .SetParameter("Censor", censor)
                    .SetParameter("CensorUserId", censorUserId)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int UpdateReportForHomepage(int leafId, string docIds, bool homepage)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("UpdateReportForHomepage")
                    .ReplaceText("{0}", docIds)
                    .SetParameter("LeafId", leafId)
                    .SetParameter("HomePage", homepage)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int UpdateReportForTop(int leafId, string docIds, string startDate, string endDate)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("UpdateReportForTop")
                    .ReplaceText("{0}", docIds)
                    .SetParameter("LeafId", leafId)
                    .SetParameter("TopStartDate", startDate)
                    .SetParameter("TopEndDate", endDate)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int UpdateReportForUnTop(int leafId, string docIds)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("UpdateReportForUnTop")
                    .ReplaceText("{0}", docIds)
                    .SetParameter("LeafId", leafId)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool CheckReportTag(long docId, string tagName)
        {
            object obj = SqlMapDAL.CreateNameQuery("CheckReportTag")
                 .SetParameter("DocId", docId)
                 .SetParameter("TagName", tagName)
                 .ExecuteScalar();
            return obj == null;
        }

        public int InsertReportTag(long docId, string tagName)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("InsertReportTag")
                    .SetParameter("DocId", docId)
                    .SetParameter("TagName", tagName)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void DelReportTag(long docId, string tagName)
        {
            SqlMapDAL.CreateNameQuery("DelReportTag")
                 .SetParameter("DocId", docId)
                 .SetParameter("TagName", tagName)
                 .ExecuteNonQuery();
        }

        public List<ReportTagInfo> GetReportTagList(long docId)
        {
            return SqlMapDAL.CreateNameQuery("GetReportTags")
                .SetParameter("DocId", docId)
                .ListEntity<ReportTagInfo>();
        }

        public List<ReportTagInfo> GetAllReportTagList(int leafId)
        {
            return SqlMapDAL.CreateNameQuery("GetAllReportTags").SetParameter("LeafId", leafId).ListEntity<ReportTagInfo>();
        }

        public ReportDetailInfo GetReportDetailInfo(long docId,int leafId)
        {
            return SqlMapDAL.CreateNameQuery("GetReportInfo").SetParameter("DocId", docId).SetParameter("LeafId", leafId).Entity<ReportDetailInfo>();
        }

        public ReportDetailInfo GetReportInfoByDocId(long docId)
        {
            return SqlMapDAL.CreateNameQuery("GetReportInfoByDocId").SetParameter("DocId", docId).ListEntity<ReportDetailInfo>().FirstOrDefault();
        }
        public bool CheckReportSubject(string subject,long docId)
        {
            object obj = SqlMapDAL.CreateNameQuery("CheckReportSubject")
                 .SetParameter("Subject", subject)
                 .SetParameter("DocId", docId)
                 .ExecuteScalar();
            return obj == null;
        }

        public List<ReportFileInfo> GetReportFileList(long docId)
        {
            return SqlMapDAL.CreateNameQuery("GetReportFileList")
                .SetParameter("DocId", docId)
                .ListEntity<ReportFileInfo>();
        }

        public int AddReportFile(long docId, int fileId, string aliasName,int docFileTypeId)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("InsertReportFile")
                    .SetParameter("DocId", docId)
                    .SetParameter("FileId", fileId)
                    .SetParameter("AliasName", aliasName)
                    .SetParameter("DocFileTypeId",docFileTypeId)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int UpdateReportFileInfo(long docId, int fileId, string aliasName, int orderNumber)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("UpdateReportFile")
                    .SetParameter("DocId", docId)
                    .SetParameter("FileId", fileId)
                    .SetParameter("AliasName", aliasName)
                    .SetParameter("OrderNumber", orderNumber)
                    .ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void DelReportFile(long docId, int fileId)
        {
            SqlMapDAL.CreateNameQuery("DelReportFile")
                 .SetParameter("FileId", fileId)
                 .SetParameter("DocId", docId)
                 .ExecuteNonQuery();
        }

        #endregion
        #region Channel
        public bool AddleafTag(int leafid, string tagName, int orderNumber=0)
        {

            try
            {
                SqlMapDAL.CreateNameQuery("AddleafTag")
                  .SetParameter("TagName", tagName)
                  .SetParameter("LeafId", leafid)
                  .SetParameter("OrderNumber", orderNumber)
                  .ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }
        public bool AddleafTags(int leafid, List<string> tagNames)
        {

            try
            { 
                string sql = "";
                foreach (string  tag in tagNames)
                {
                     sql += "select  " + leafid + ",'" + tag + "' ,0, 0 union ";
                }
               sql = sql.Substring(0, sql.Length - 7);
               SqlMapDAL.CreateNameQuery("AddleafTags") .ReplaceText("{0}", sql) .ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }



        public bool RemoveleafTag(int leafid, string tagName)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("RemoveleafTag")
                  .SetParameter("TagName", tagName)
                  .SetParameter("LeafId", leafid)
                  .ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }
        public bool CheckLeafTagName(int leafid, string tagName)
        {
            var obj = SqlMapDAL.CreateNameQuery("CheckLeafTagName")
                  .SetParameter("TagName", tagName)
                  .SetParameter("LeafId", leafid)
                  .ExecuteScalar();
            return obj == null ? false : true;
      
        }

        public bool EditLeafTag(int leafid, string newTagName, string oldTagName, int OrderNumber=0)
        {
            try
            {
                SqlMapDAL.CreateNameQuery("updateleafTag")
                  .SetParameter("newTagName", newTagName)
                  .SetParameter("oldTagName", oldTagName)
                  .SetParameter("LeafId", leafid)
                  .SetParameter("OrderNumber", OrderNumber)
                  .ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }



        public List<IntTreeNodeInfo> GetChannelTree(int parentId, bool onlyCensor)
        {
            ChannelBLL bll=new ChannelBLL();
            return bll.GetChannelTree(parentId, onlyCensor);

        }
        //jelly add 
        public List<IntTreeNodeInfo> GetChannelTreeWithLeafs(int parentId, bool onlyCensor)
        {
            ChannelBLL bll = new ChannelBLL();
            return bll.GetChannelTreeWithLeafs(parentId, onlyCensor);
        }
        //jelly add 
         public List<int> TansforNode2Leafs(List<IntTreeNodeInfo> nodelist)
        {
           ChannelBLL bll = new ChannelBLL();
           return bll.TansforNode2Leafs(nodelist);
         }
        public List<ChannelInfo> GetLeafs(int chnId, bool onlyCensor)
        {
            return
               SqlMapDAL.CreateNameQuery("GetLeafs")
                   .SetParameter("chnId", chnId)
                   .SetParameter("censor", onlyCensor)
                   .ListEntity<ChannelInfo>();
        }
        public List<ChannelInfo> GetChildrenChannel(int chnid, bool onlyCensor)
        {

            return
                SqlMapDAL.CreateNameQuery("GetChildrenChannelInfo")
                    .SetParameter("parentid", chnid)
                    .SetParameter("censor", onlyCensor)
                    .ListEntity<ChannelInfo>();
        }
       

        public List<ChannelInfo> GetAllChannelLeafs(int pChnid, bool onlyCensor)
        {
            ChannelBLL bll = new ChannelBLL();
            List<ChannelInfo> allLeaf = new List<ChannelInfo>();
            allLeaf = bll.GetAllChannelLeafs(pChnid, onlyCensor);
            return allLeaf;

        }







        public ChannelInfo GetChannelInfo(int chnId)
        {
            return
               SqlMapDAL.CreateNameQuery("GetChannelInfo")
                   .SetParameter("chnid", chnId)
                   .Entity<ChannelInfo>();
        }

        public ChannelInfo GetLeafInfo(int chnId, int leafid)
        {
            return
               SqlMapDAL.CreateNameQuery("GetChannelLeafInfo")
                   .SetParameter("chnid", chnId).SetParameter("leafid",leafid)
                   .Entity<ChannelInfo>();
        }
        
        //新增栏目 
        public int InsertChannelInfo(string chnNam, int parentId, int OrderNumber, bool censor, string comment, bool isLeaf)
        {
            if (isLeaf)
            {//新增叶子，新增叶子表关系表（内部事务）

                int tt = (int)SqlMapDAL.CreateNameQuery("InsertLeaf",true )
               .SetParameter("LeafName", chnNam)
               .SetParameter("ChnId", parentId)
               .SetParameter("OrderNumber", OrderNumber)
               .SetParameter("Censor", censor ? 1 : 0)
               .SetParameter("Comment", comment).ExecuteScalar();
                return tt;
            }
            else {//新增栏目  
              string xxx =SqlMapDAL.CreateNameQuery("InsertChannel")
              .SetParameter("ChnName", chnNam)
              .SetParameter("parentId", parentId)
              .SetParameter("OrderNumber", OrderNumber)
              .SetParameter("Censor", censor ? 1 : 0)
              .SetParameter("Comment", comment).ExecuteScalar().ToString();
              int tt = Convert.ToInt32(xxx);
               return tt;
            }
        }
        //修改栏目 
        public bool UpdateChannelInfo( int ChnId , string chnNam, int parentId, int OrderNumber, bool censor, string comment)
        {
            bool result = false;
            try {
                
                  SqlMapDAL.CreateNameQuery("UpdateChannel")
                                  .SetParameter("ChnId", ChnId)
                                  .SetParameter("ChnName", chnNam)
                                  .SetParameter("parentId", parentId)
                                  .SetParameter("OrderNumber", OrderNumber)
                                  .SetParameter("censor", censor ? 1 : 0)
                                  .SetParameter("comment", comment)
                                  .ExecuteNonQuery();
                   result = true;
            }
            catch (Exception e) { result = false; }
            return result; 
        }
        public bool UpdateLeafInfo(int oldparentID, int ChnId,
           string chnNam, int parentId, int OrderNumber, bool censor, string comment)
        {
            bool result = false;
            try
            {
                   SqlMapDAL.CreateNameQuery("UpdateLeaf", true)
                                     .SetParameter("parentIDChnId", oldparentID)
                                     .SetParameter("LeafId", ChnId)
                                     .SetParameter("LeafName", chnNam)
                                     .SetParameter("OrderNumber", OrderNumber)
                                     .SetParameter("ChnId", parentId)
                                     .SetParameter("censor", censor ? 1 : 0)
                                     .SetParameter("comment", comment)
                                     .ExecuteNonQuery();

                    result = true;
            }
            catch (Exception e) { result = false; }
            return result;
        }
        //删除栏目（删子栏目(状态置1)、本栏目包含的叶子关系状态置1 （子栏目及包含的叶子关系自动不可见）
        public bool DelChannelInfo(int chnId) 
        {

            bool result = false;
            try
            {
                    SqlMapDAL.CreateNameQuery("DelChannel",true)
                                .SetParameter("ChnId", chnId)
                                .ExecuteNonQuery();
                    result = true;
            }
            catch (Exception e) { result = false; }
            return result; 
        }

        //删除叶子(要明确哪个栏目下的叶子， 关系状态置1。  叶子表状态不能改：防止该叶子属于多个频道)
        public bool DeltLeafInfo(int chnId, int leafid) 
        {
            bool result = false;
            try
            {
                SqlMapDAL.CreateNameQuery("DelLeaf")
                            .SetParameter("ChnId", chnId)
                            .SetParameter("LeafId", leafid)
                            .ExecuteNonQuery();
                result = true;
            }
            catch (Exception e) { result = false; }
            return result; 
        }

        public byte[] GetChnnalPhoto(int ChnId,bool IsLeaf)
        {
            return (byte[])SqlMapDAL.CreateNameQuery("GetChnnalPhoto")
                .SetParameter("ChnOrLeafId", ChnId)
                .SetParameter("IsChn", IsLeaf?0: 1)
                .ExecuteScalar();
        }

        public void UploadChnnalPhoto(int ChnId,bool IsLeaf, byte[] photo)
        {
            SqlMapDAL.CreateNameQuery("UploadChnnalPhoto", true)
                .SetParameter("ChnOrLeafId", ChnId)
                .SetParameter("IsChn", IsLeaf ? 0 : 1)
                .SetParameter("Photo", photo)
                .ExecuteNonQuery();

        }

        public bool CheckName(int parentId,  string cName, bool IsLeaf)
        {
             try {
                 if (IsLeaf) { 
                     object id = SqlMapDAL.CreateNameQuery("CheckLeafName")
                    .SetParameter("LeafName", cName)
                    .SetParameter("pChnID", parentId).ExecuteScalar();
                     return id == null ? true : false;
                 }
                 else
                 {
                     object id = SqlMapDAL.CreateNameQuery("CheckChannelName")
                    .SetParameter("ChnName", cName)
                    .SetParameter("pChnID", parentId).ExecuteScalar();
                     return id == null ? true : false;
                 }
                
            }
            catch(Exception e){
                return false;
            }
        }
        #endregion

        public List<int> GetLeafIdsByDocId(Int64 docId)
        {
            return SqlMapDAL.CreateNameQuery("GetLeafIdsByDocId").SetParameter("docId", docId).ListValueObject<int>();
        }
        /// <summary>
        /// 数据文章编号，返回模块编号和LeafId。
        /// </summary>
        /// <param name="docId"></param>
        /// <returns>Dictionary<LeafId,List<ModuleId>></ModuleId></returns>
        public Dictionary<int, List<int>> GetRModuleLeafs(Int64 docId)
        {
            List<int> leafIds = GetLeafIdsByDocId(docId);
            Dictionary<int,List<int>> result=new Dictionary<int, List<int>>();
            ModuleBLL bll=new ModuleBLL();
            foreach (var leafId in leafIds)
            {
                List<int> modules = bll.GetDocMuleInfo(leafId);
                result.Add(leafId,modules);
            }
            return result;
        }

        public List<ReportHitInfo> GetReportetDocByHit(int period, int topNumber)
        {
            //string key = period + "_" + topNumber;
            //object obj = CacheUtility.GetInstance().Get(key,CacheType.文件);
            //if (obj != null) return obj as List<ReportHitInfo> ;


            List<ReportHitInfo> tt =new List<ReportHitInfo>();
            tt = SqlMapDAL.CreateNameQuery("GetReportetDocByHit")
            .SetParameter("period", period)
            .SetParameter("topNumber", topNumber).ListEntity<ReportHitInfo>();
            List<ReportHitInfo> result = new List<ReportHitInfo>();
            result.AddRange(tt);
              if (tt.Count < topNumber)
               {
                   int  addnum=topNumber-tt.Count;
                    List<ReportHitInfo> ttadd =new List<ReportHitInfo>();
                    ttadd = SqlMapDAL.CreateNameQuery("GetReportetDocByHitAdd")
                   .SetParameter("topNumber", addnum).ListEntity<ReportHitInfo>();
                    result.AddRange(ttadd);
               }
          //  CacheUtility.GetInstance().Add(key,result,new TimeSpan(0,1,0),CacheType.文件);

              return result;
        }

        #region 文章导入
        public void AddDocImportRecord(long docId, string subject, long drcnetDocId)
        {
            SqlMapDAL.CreateNameQuery("AddDocImportRecord")
                .SetParameter("docid", docId)
                .SetParameter("subject", subject)
                .SetParameter("drcnetDocId", drcnetDocId)
                .ExecuteNonQuery();
        }

        public List<long> GetImportedDocDRCNetDocIds()
        {
            List<long> tt = SqlMapDAL.CreateNameQuery("GetImportedDocDRCNetDocIds").ListValueObject<long>();
            return tt;
        }

        #endregion
    }
}
