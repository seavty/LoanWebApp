//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoanWebApp.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class sm_doc
    {
        public int docs_docID { get; set; }
        public string docs_Deleted { get; set; }
        public Nullable<int> docs_CreatedBy { get; set; }
        public Nullable<System.DateTime> docs_CreatedDate { get; set; }
        public Nullable<int> docs_UpdatedBy { get; set; }
        public Nullable<System.DateTime> docs_UpdatedDate { get; set; }
        public Nullable<int> docs_WorkflowID { get; set; }
        public Nullable<int> docs_WorkflowItemID { get; set; }
        public Nullable<int> docs_ZoneID { get; set; }
        public string docs_Name { get; set; }
        public string docs_Description { get; set; }
        public Nullable<int> docs_TableID { get; set; }
        public string docs_Value { get; set; }
        public string docs_FilePath { get; set; }
        public string docs_Default { get; set; }
    }
}
