﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartSchool.API.PlugIn;
using SHSchool.Data;
using System.Xml.Linq;

namespace UpdateRecordModule_KHSH_D.ImportExport
{
    /// <summary>
    /// 匯出學籍異動
    /// </summary>
    public class ExportStudUpdateRecordList: SmartSchool.API.PlugIn.Export.Exporter
    {
        /// <summary>
        /// 可匯出項目
        /// </summary>
        List<string> _ExportList;

        public ExportStudUpdateRecordList()
        {
            _ExportList = new List<string>();
            this.Image = null;
            this.Text = "匯出學籍異動";
            _ExportList.Add("學年度");
            _ExportList.Add("學期");
            _ExportList.Add("異動年級");
            _ExportList.Add("異動代碼");
            _ExportList.Add("原因及事項");
            _ExportList.Add("異動日期");
            _ExportList.Add("備註");
            _ExportList.Add("班別");
            _ExportList.Add("科別");
            _ExportList.Add("特殊身分代碼");
            _ExportList.Add("異動姓名");
            _ExportList.Add("異動學號");
            _ExportList.Add("異動身分證字號");
            _ExportList.Add("更正後資料");
            _ExportList.Add("異動生日");
            _ExportList.Add("異動性別");
            _ExportList.Add("異動身分證註記");
            _ExportList.Add("備查日期");
            _ExportList.Add("備查文號");
            _ExportList.Add("核准日期");
            _ExportList.Add("核准文號");
            _ExportList.Add("應畢業學年度");
            _ExportList.Add("更正後身分證註記");
            _ExportList.Add("借讀學校代碼");
            _ExportList.Add("借讀科別代碼");
            _ExportList.Add("申請開始日期");
            _ExportList.Add("申請結束日期");
            _ExportList.Add("實際開始日期");
            _ExportList.Add("實際結束日期");
            _ExportList.Add("狀態");
        }
        public override void InitializeExport(SmartSchool.API.PlugIn.Export.ExportWizard wizard)
        {
            wizard.ExportableFields.AddRange(_ExportList);
            wizard.ExportPackage += delegate(object sender, SmartSchool.API.PlugIn.Export.ExportPackageEventArgs e)
            {
                // 取得異動代碼
                XElement _UpdateCode = DAL.DALTransfer.GetUpdateCodeList();
                List<string> UpdateCodeList = (from elm in _UpdateCode.Elements("異動") where elm.Element("分類").Value == "學籍異動" select elm.Element("代號").Value).ToList();
                // 取得學生學籍異動
                List<SHUpdateRecordRecord> updateRecList = (from data in SHUpdateRecord.SelectByStudentIDs(e.List) where UpdateCodeList.Contains(data.UpdateCode) select data).ToList();
                Dictionary<string, string> StudentStatusDict = DAL.FDQuery.GetAllStudentStatus1Dict();

                foreach (SHUpdateRecordRecord rec in updateRecList)
                {
                    RowData row = new RowData();
                    row.ID = rec.StudentID;
                    foreach (string field in e.ExportFields)
                    {
                        if (wizard.ExportableFields.Contains(field))
                        {
                            switch (field)
                            {
                                case "學年度":
                                    if (rec.SchoolYear.HasValue)
                                        row.Add(field, rec.SchoolYear.Value.ToString()); break;
                                case "學期":
                                    if (rec.Semester.HasValue)
                                        row.Add(field, rec.Semester.Value.ToString()); break;
                                case "異動年級": row.Add(field, rec.GradeYear); break;
                                case "異動代碼": row.Add(field, rec.UpdateCode); break;
                                case "原因及事項": row.Add(field, rec.UpdateDescription); break;
                                case "異動日期": row.Add(field, rec.UpdateDate); break;
                                case "備註": row.Add(field, rec.Comment); break;
                                case "班別": row.Add(field, rec.ClassType); break;
                                case "科別": row.Add(field, rec.Department); break;
                                case "特殊身分代碼": row.Add(field, rec.SpecialStatus); break;
                                case "應畢業學年度": row.Add(field, rec.ExpectGraduateSchoolYear); break;
                                case "異動姓名": row.Add(field, rec.StudentName); break;
                                case "異動學號": row.Add(field, rec.StudentNumber); break;
                                case "異動身分證字號": row.Add(field, rec.IDNumber); break;
                                case "更正後資料": row.Add(field, rec.NewData); break;
                                case "異動生日": row.Add(field, rec.Birthdate); break;
                                case "異動身分證註記": row.Add(field, rec.IDNumberComment); break;
                                case "更正後身分證註記": row.Add(field, rec.Comment2); break;
                                case "舊科別代碼": row.Add(field, rec.OldDepartmentCode); break;
                                case "舊班別": row.Add(field, rec.OldClassType); break;
                                case "異動性別": row.Add(field, rec.Gender); break;
                                case "備查日期": row.Add(field, rec.LastADDate); break;
                                case "備查文號": row.Add(field, rec.LastADNumber); break;
                                case "核准日期": row.Add(field, rec.ADDate); break;
                                case "核准文號": row.Add(field, rec.ADNumber); break;
                                case "借讀學校代碼": row.Add(field, rec.Code7SchoolCode); break;
                                case "借讀科別代碼": row.Add(field, rec.Code7DeptCode); break;
                                case "申請開始日期": row.Add(field, rec.Code71BeginDate); break;
                                case "申請結束日期": row.Add(field, rec.Code71EndDate); break;
                                case "實際開始日期": row.Add(field, rec.Code72BeginDate); break;
                                case "實際結束日期": row.Add(field, rec.Code72EndDate); break;
                                case "狀態":
                                    if (StudentStatusDict.ContainsKey(rec.StudentID))
                                        row.Add(field, StudentStatusDict[rec.StudentID]); break;
                            }
                        }
                    }
                    e.Items.Add(row);
                }
            };
        }
    }
}
