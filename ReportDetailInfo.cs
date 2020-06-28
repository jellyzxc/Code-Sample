using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using eDRCNet.EF.Entity;

namespace eDRCNet.IntraCountyEconomy.Model.DF
{
    [Serializable]
    [DataContract]
    public partial class ReportDetailInfo : EntityBase
    {
        public ReportDetailInfo()
        {
            this.EntityMap = EntityMapType.SqlMap;
        }

        protected override void SetPropertyNames()
        {
            PropertyNames = new string[]
            {
                "ContentId",
                "DocId", 
                "Subject", 
                "Author", 
                "Source", 
                "UserId",
                "Keywords", 
                "Summary", 
                "ContentSource",
                "LeafId",  
                "DelivedDate",  
                "Content", 
                "PageNo" ,
                "CensorCompany"
            };
        }
        /// <summary>
        /// 内容ID
        /// </summary>
        [DataMember]
        public Int64 ContentId { get { return getProperty<Int64>("ContentId"); } set { setProperty("ContentId", value); } }
        /// <summary>
        /// 报告ID
        /// </summary>
        [DataMember]
        public Int64 DocId { get { return getProperty<Int64>("DocId"); } set { setProperty("DocId", value); } }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Subject { get { return getProperty<string>("Subject"); } set { setProperty("Subject", value); } }

        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public string Author { get { return getProperty<string>("Author"); } set { setProperty("Author", value); } }
        /// <summary>
        /// 发布单位--jelly ：应詹涛要求添加
        /// </summary>
        [DataMember]
        public string CensorCompany { get { return getProperty<string>("CensorCompany"); } set { setProperty("CensorCompany", value); } }
        /// <summary>
        /// 来源
        /// </summary>
        [DataMember]
        public string Source { get { return getProperty<string>("Source"); } set { setProperty("Source", value); } }

        /// <summary>
        /// 关键词
        /// </summary>
        [DataMember]
        public string Keywords { get { return getProperty<string>("Keywords"); } set { setProperty("Keywords", value); } }

        /// <summary>
        /// 摘要
        /// </summary>
        [DataMember]
        public string Summary { get { return getProperty<string>("Summary"); } set { setProperty("Summary", value); } }

        /// <summary>
        /// 内容类型
        /// </summary>
        [DataMember]
        public string ContentSource { get { return getProperty<string>("ContentSource"); } set { setProperty("ContentSource", value); } }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public int UserId { get { return getProperty<int>("UserId"); } set { setProperty("UserId", value); } }

        /// <summary>
        /// 子栏目ID
        /// </summary>
        [DataMember]
        public int LeafId { get { return getProperty<int>("LeafId"); } set { setProperty("LeafId", value); } }

        /// <summary>
        /// 发布日期
        /// </summary>
        [DataMember]
        public DateTime DelivedDate { get { return getProperty<DateTime>("DelivedDate"); } set { setProperty("DelivedDate", value); } }

        /// <summary>
        /// HTML格式报告内容
        /// </summary>
        [DataMember]
        public string Content { get { return getProperty<string>("Content"); } set { setProperty("Content", value); } }

        /// <summary>
        /// 报告页数
        /// </summary>
        [DataMember]
        public int PageNo { get { return getProperty<int>("PageNo"); } set { setProperty("PageNo", value); } }
    }
}
