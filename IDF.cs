
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using eDRCNet.IntraCountyEconomy.Model;
using eDRCNet.IntraCountyEconomy.Model.DF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace eDRCNet.IntraCountyEconomy.WCFService.DF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IDF
    {
        #region DFDocComment 评论相关
        [OperationContract]
        //获取其评论列表
        List<DocCommentInfo> GetCommentListByChnId(int chnId, int curPage, int pagesize, out int total);
        [OperationContract]
        //获取其评论列表
        List<DocCommentInfo> GetCommentListByDocId(long DocId, int curPage, int pagesize, out int total);
        [OperationContract]
        //根据发帖人ID获取其发表的评论列表(---我发表的评论----)
        List<DocCommentInfo> GetCommentListByUserId(int UserId, int curPage, int pagesize, out int total);
        [OperationContract]
        //根据发帖人ID获取与其相关的评论列表（----评论过的文章收到的评论们----）
        List<DocCommentInfo> GetRelevantCommentListByUserId(int UserId, int curPage, int pagesize, out int total);
        [OperationContract]
        //根据帖子内容模糊查找----评论管理中使用
        List<DocCommentInfo> GetCommentListByContent(string Content, int curPage, int pagesize, out int total);
        [OperationContract]
        //新增帖子 返回帖子ID
        int InsertComment(long DocId, bool IsAnonymous, int userId, string UserName, string Content, long ParentId);
        [OperationContract]
        //删除帖子(针对该贴的回复也同时删掉)
        void DelCommentByCommentId(long CommentId);
        [OperationContract]
        //查看帖子详情
        DocCommentInfo GetDocCommentInfo(long CommentId);
       
       #endregion


        #region Report 评价报告相关
        [OperationContract]
        List<ReportBasicInfo> GetReportListByChnIdForHome(int chnId, int num);

        [OperationContract]
        List<ReportBasicInfo> GetTopCensorReportListByLeafId(int leafId);
        /// <summary>
        /// 获取报告列表,根据标题查询，根据标签查询，高级查询
        /// </summary>
        /// <param name="leafId"></param>
        /// <param name="curPage"></param>
        /// <param name="pagesize"></param>
        /// <param name="isAsc"></param>
        /// <param name="total"></param>
        /// <param name="condition"></param>
        /// <param name="tagName"></param>
        /// <param name="orderbyField"></param>
        /// <returns></returns>
        [OperationContract]

        List<ReportBasicInfo> GetReportListByLeafId(int leafId, int curPage, int pagesize, Dictionary<string,string> condition, string tagName, string orderbyField, bool isAsc, out int total);


        [OperationContract]
        List<ReportBasicInfo> GetCensorReportListByLeafId(int leafId, int curPage, int pagesize, Dictionary<string, string> condition, string orderbyField, bool isAsc, out int total);
        [OperationContract]
        List<ReportBasicInfo> GetCensorReportListByLeafIdandTagName(int leafId, string TagName, int curPage, int pagesize, Dictionary<string, string> condition, string orderbyField, bool isAsc, out int total);

        [OperationContract]
        List<ReportBasicInfo> GetCensorReportListByLeafIdWithOutTagName(int leafId, int curPage, int pagesize, Dictionary<string, string> condition, string orderbyField, bool isAsc, out int total);
        [OperationContract]
        List<ReportBasicInfo> GetReportListByChnId(int chnId, int curPage, int pagesize, Dictionary<string, string> condition, string tagName, string orderbyField, bool isAsc, out int total);

        /// <summary>
        /// 新增报告
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [OperationContract]
        string InsertOrUpdateReportInfo(ReportDetailInfo info);

        /// <summary>
        /// 删除报告
        /// </summary>
        /// <param name="leafId"></param>
        /// <param name="docIds"></param>
        /// <returns></returns>
        [OperationContract]
        void DelReport(int leafId, string docIds);

        /// <summary>
        /// 修改报告内容
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="content"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [OperationContract]
        int UpdateReportContent(long docId, string content,int pageNo);

        /// <summary>
        /// 发布报告,取消发布报告
        /// </summary>
        /// <param name="leafId"></param>
        /// <param name="docIds"></param>
        /// <param name="censor"></param>
        /// <param name="censorUserId"></param>
        /// <returns></returns>
        [OperationContract]
        int UpdateReportForCensor(int leafId, string docIds,Boolean censor,int censorUserId);

        /// <summary>
        /// 首页显示,取消首页显示
        /// </summary>
        /// <param name="leafId"></param>
        /// <param name="docIds"></param>
        /// <param name="homepage"></param>
        /// <returns></returns>
        [OperationContract]
        int UpdateReportForHomepage(int leafId, string docIds, Boolean homepage);

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="leafId"></param>
        /// <param name="docIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        int UpdateReportForTop(int leafId, string docIds, string startDate, string endDate);

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="leafId"></param>
        /// <param name="docIds"></param>
        /// <returns></returns>
        [OperationContract]
        int UpdateReportForUnTop(int leafId, string docIds);

        /// <summary>
        /// 校验标签是否已经添加
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [OperationContract]
        bool CheckReportTag(long docId, string tagName);

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertReportTag(long docId, string tagName);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [OperationContract]
        void DelReportTag(long docId, string tagName);

        /// <summary>
        /// 获取报告标签列表
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ReportTagInfo> GetReportTagList(long docId);

        /// <summary>
        /// 获取全部报告标签列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ReportTagInfo> GetAllReportTagList(int leafId);

        /// <summary>
        /// 查看报告详情
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="leafId"></param>
        /// <returns></returns>
        [OperationContract]
        ReportDetailInfo GetReportDetailInfo(long docId,int leafId);

        //查看报告详情--采用查询结果的第一个
        [OperationContract]
        ReportDetailInfo GetReportInfoByDocId(long docId);
        /// <summary>
        /// 校验报告标题是否已经存在
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="docId"></param>
        /// <returns></returns>
        [OperationContract]
        bool CheckReportSubject(string subject, long docId);

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ReportFileInfo> GetReportFileList(long docId);

        /// <summary>
        /// 插入文章文件
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="fileId"></param>
        /// <param name="aliasName"></param>
        /// <param name="docFileTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        int AddReportFile(long docId, int fileId, string aliasName, int docFileTypeId);

        /// <summary>
        /// 修改文件信息
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="fileId"></param>
        /// <param name="aliasName"></param>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        [OperationContract]
        int UpdateReportFileInfo(long docId, int fileId, string aliasName, int orderNumber);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [OperationContract]
        void DelReportFile(long docId, int fileId);


        [OperationContract]
        bool AddleafTag(int leafid, string tagName, int OrderNumber=0);
        [OperationContract]
        bool RemoveleafTag(int leafid, string tagName);
        [OperationContract]
        bool CheckLeafTagName(int leafid, string tagName);

        [OperationContract]
        bool AddleafTags(int leafid, List<string> tagNames);

        [OperationContract]
        bool EditLeafTag(int leafid, string newTagName, string oldTagName, int OrderNumber = 0);
        [OperationContract]
        List<ReportHitInfo> GetReportetDocByHit(int period, int topNumber);

        [OperationContract]
        void AddDocImportRecord(long docId, string subject, long drcnetDocId);

        [OperationContract]
        List<long> GetImportedDocDRCNetDocIds();
        #endregion

        #region Channel
        /// <summary>
        /// 获取栏目树
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="onlyCensor"></param>
        /// <returns></returns>
        [OperationContract]
        List<IntTreeNodeInfo> GetChannelTree(int parentId, bool onlyCensor);


        [OperationContract]
        List<IntTreeNodeInfo> GetChannelTreeWithLeafs(int parentId, bool onlyCensor);
        [OperationContract]
        List<int> TansforNode2Leafs(List<IntTreeNodeInfo> nodelist);
        /// <summary>
        /// 获取频道
        /// </summary>
        /// <param name="chnId"></param>
        /// <param name="onlyCensor"></param>
        /// <returns></returns>
        [OperationContract]
        List<ChannelInfo> GetLeafs(int chnId, bool onlyCensor);

        [OperationContract]
        List<int> GetLeafIdsByDocId(Int64 docId);

        /// <summary>
        /// 权限校验的时候使用
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<int, List<int>> GetRModuleLeafs(Int64 docId);

            /// <summary>
        /// 获取子栏目
        /// </summary>
        /// <param name="chnid"></param>
        /// <param name="onlyCensor"></param>
        /// <returns></returns>
        [OperationContract]
        List<ChannelInfo> GetChildrenChannel(int chnid, bool onlyCensor);

        /// <summary>
        /// 获取栏目基本信息
        /// </summary>
        /// <param name="chnId"></param>
        /// <returns></returns>
        [OperationContract]
        ChannelInfo GetChannelInfo(int chnId);
        /// <summary>
        /// 获取叶子节点信息（父ID是关系表中对应的频道）
        /// </summary>
        /// <param name="chnId"></param>
        /// <param name="leafid"></param>
        /// <returns></returns>
        [OperationContract]
        ChannelInfo GetLeafInfo(int chnId,int leafid);
        //新增栏目或叶子 
        [OperationContract]
        int InsertChannelInfo(string chnNam, int parentId, int OrderNumber, bool censor, string comment, bool isLeaf);
        //修改栏目 
        [OperationContract]
        //bool UpdateChannelInfo(int parentID, ChannelInfo channelInfo);
        bool UpdateChannelInfo(int ChnId, string chnNam, int parentId, int OrderNumber, bool censor, string comment);
       //修改叶子 
        [OperationContract]
        bool UpdateLeafInfo(int oldparentID, int ChnId, string chnNam, int parentId, int OrderNumber, bool censor, string comment);
        //删除栏目 
        [OperationContract]
        bool DelChannelInfo(int chnId);
        //删除叶子 
        [OperationContract]
        bool DeltLeafInfo(int chnId, int leafid);
        [OperationContract]
        byte[] GetChnnalPhoto(int ChnId,bool IsLeaf);
        [OperationContract]
        void UploadChnnalPhoto(int ChnId, bool IsLeaf, byte[] photo);
        //获取栏目下的所有叶子
        [OperationContract]
        List<ChannelInfo> GetAllChannelLeafs(int pChnid, bool onlyCensor );

        [OperationContract]
        //新增节点时判断该父目录下是否存在该节点
        bool CheckName(int parentId, string cName, bool IsLeaf);

        #endregion


    }


}
