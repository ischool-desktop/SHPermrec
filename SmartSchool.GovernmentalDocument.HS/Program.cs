using System;
using System.Collections.Generic;
using System.Xml;
using SmartSchool.Customization.PlugIn;
using SmartSchool.Customization.PlugIn.ExtendedContent;
using SmartSchool.Adaatper;
using SmartSchool.ExceptionHandler;
using FISCA.Presentation;
using SmartSchool.StudentRelated.RibbonBars.Export;
using SmartSchool.StudentRelated.RibbonBars.Import;
using SmartSchool.GovernmentalDocument.ImportExport;

namespace SmartSchool.GovernmentalDocument
{
    public class Program
    {
        [MainMethod()]
        [FISCA.MainMethod("����¾�鶡�����y���ʨt��")]
        public static void Main()
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "���_�U���U���U�U��"))) return;

            //�����
            List<Customization.PlugIn.ExtendedContent.IContentItem> _items = new List<Customization.PlugIn.ExtendedContent.IContentItem>();

            List<Type> _type_list = new List<Type>(new Type[]{
                //// ���ʸ�ƶ���(��)����
                //typeof(Content.UpdatePalmerwormItem)
            });

            foreach (Type type in _type_list)
            {
                if (!Attribute.IsDefined(type, typeof(SmartSchool.AccessControl.FeatureCodeAttribute)) || CurrentUser.Acl[type].Viewable)
                {
                    try
                    {
                        IContentItem item = type.GetConstructor(Type.EmptyTypes).Invoke(null) as IContentItem;
                        _items.Add(item);
                    }
                    catch (Exception ex) {BugReporter.ReportException(ex, false); }
                }
            }
            foreach (Customization.PlugIn.ExtendedContent.IContentItem var in _items)
            {
                K12.Presentation.NLDPanels.Student.AddDetailBulider(new ContentItemBulider(var));
            }

            //UserControl1 updateRecord = new UserControl1();
            //BaseItem item = c.ProcessRibbon.Items[0];

            //SmartSchool.Customization.PlugIn.GeneralizationPluhgInManager<BaseItem>.Instance[@"�ǥ�\���w"].Add(item);
            //SmartSchool.Customization.PlugIn.GeneralizationPluhgInManager<BaseItem>.Instance.Add(@"�ǥ�\���y�@�~", updateRecord);

            // �¥\��
            //�W�URibbon
            //new Process.NameList();

            // �妸���~���ʥ\��
            new Process.BatchUpdateRecord();

            RibbonBarButton rbItemExport = K12.Presentation.NLDPanels.Student.RibbonBarItems["��Ʋέp"]["�ץX"];

            #region �ץX(1000708)

            //rbItemExport["���ʬ����ץX"]["�ץX�s�Ͳ���"].Enable = CurrentUser.Acl["Button0200"].Executable;
            //rbItemExport["���ʬ����ץX"]["�ץX�s�Ͳ���"].Click += delegate
            //{
            //    new ExportStudent(new ExportNewStudentsUpdateRecord()).ShowDialog();
            //};

            //rbItemExport["���ʬ����ץX"]["�ץX��J����"].Enable = CurrentUser.Acl["Button0200"].Executable;
            //rbItemExport["���ʬ����ץX"]["�ץX��J����"].Click += delegate
            //{
            //    new ExportStudent(new ExportTransferSchoolStudentsUpdateRecord()).ShowDialog();
            //};

            //rbItemExport["���ʬ����ץX"]["�ץX���y����"].Enable = CurrentUser.Acl["Button0200"].Executable;
            //rbItemExport["���ʬ����ץX"]["�ץX���y����"].Click += delegate
            //{
            //    new ExportStudent(new ExprotStudentsUpdateRecord()).ShowDialog();
            //};

            //rbItemExport["���ʬ����ץX"]["�ץX���~����"].Enable = CurrentUser.Acl["Button0200"].Executable;
            //rbItemExport["���ʬ����ץX"]["�ץX���~����"].Click += delegate
            //{
            //    new ExportStudent(new ExportStudentGraduateUpdateRecord()).ShowDialog();
            //}; 
            #endregion


            RibbonBarButton rbItemImport = K12.Presentation.NLDPanels.Student.RibbonBarItems["��Ʋέp"]["�פJ"];

            #region �פJ(1000708)
            //rbItemImport["���ʬ����פJ"]["�פJ�s�Ͳ���"].Enable = CurrentUser.Acl["Button0280"].Executable;
            //rbItemImport["���ʬ����פJ"]["�פJ�s�Ͳ���"].Click += delegate
            //{
            //    new ImportStudent(new ImportNewStudentsUpdateRecord()).ShowDialog();
            //};

            //rbItemImport["���ʬ����פJ"]["�פJ��J����"].Enable = CurrentUser.Acl["Button0280"].Executable;
            //rbItemImport["���ʬ����פJ"]["�פJ��J����"].Click += delegate
            //{
            //    new ImportStudent(new ImportTransferSchoolStudentsUpdateRecord()).ShowDialog();
            //};

            //rbItemImport["���ʬ����פJ"]["�פJ���~����"].Enable = CurrentUser.Acl["Button0280"].Executable;
            //rbItemImport["���ʬ����פJ"]["�פJ���~����"].Click += delegate
            //{
            //    new ImportStudent(new ImportStudentGraduateUpdateRecord()).ShowDialog();
            //}; 
            #endregion

            //�ץX���ʬ���(1000708����)
            //SmartSchool.Customization.PlugIn.ImportExport.ExportStudent.AddProcess(new ImportExport.ExportNewStudentsUpdateRecord());
            //SmartSchool.Customization.PlugIn.ImportExport.ExportStudent.AddProcess(new ImportExport.ExportTransferSchoolStudentsUpdateRecord());
            //SmartSchool.Customization.PlugIn.ImportExport.ExportStudent.AddProcess(new ImportExport.ExprotStudentsUpdateRecord());
            //SmartSchool.Customization.PlugIn.ImportExport.ExportStudent.AddProcess(new ImportExport.ExportStudentGraduateUpdateRecord());

            //�פJ���ʬ���(1000708����)
            //SmartSchool.Customization.PlugIn.ImportExport.ImportStudent.AddProcess(new ImportExport.ImportNewStudentsUpdateRecord());
            //SmartSchool.Customization.PlugIn.ImportExport.ImportStudent.AddProcess(new ImportExport.ImportTransferSchoolStudentsUpdateRecord());
            //SmartSchool.Customization.PlugIn.ImportExport.ImportStudent.AddProcess(new ImportExport.ImportStudentGraduateUpdateRecord());
            
            //??�Q����
            //SmartSchool.Customization.PlugIn.ImportExport.ImportStudent.AddProcess(new ImportExport.ImportStudentsUpdateRecord());
            //SmartSchool.Customization.PlugIn.ImportExport.ImportStudent.AddProcess(new ImportExport.ImportStudentGraduateUpdateRecord());
            SmartSchool.Customization.Data.StudentHelper.FillingUpdateRecord += new EventHandler<SmartSchool.Customization.Data.FillEventArgs<SmartSchool.Customization.Data.StudentRecord>>(StudentHelper_FillingUpdateRecord);
        }

        private const int _PackageLimit = 500;

        private static List<T>[] SplitPackage<T>(List<T> list)
        {
            if (list.Count > 0)
            {
                int packageCount = (list.Count / _PackageLimit + 1);
                int packageSize = list.Count / packageCount + list.Count % packageCount;
                packageCount = 0;
                List<List<T>> packages = new List<List<T>>();
                List<T> packageCurrent = new List<T>();
                foreach (T var in list)
                {
                    packageCurrent.Add(var);
                    packageCount++;
                    if (packageCount == packageSize)
                    {
                        packageCount = 0;
                        packages.Add(packageCurrent);
                    }
                }
                return packages.ToArray();
            }
            else
                return new List<T>[0];
        }

        private static List<T> GetList<T>(IEnumerable<T> items)
        {
            List<T> list = new List<T>();
            list.AddRange(items);
            return list;
        }

        static void StudentHelper_FillingUpdateRecord(object sender, SmartSchool.Customization.Data.FillEventArgs<SmartSchool.Customization.Data.StudentRecord> e)
        {
            //���o�N�X��Ӫ�
            XmlElement updateCodeMappingElement = SmartSchool.Feature.Basic.Config.GetUpdateCodeSynopsis().GetContent().BaseElement;
            //���妸�B�z
            foreach (List<SmartSchool.Customization.Data.StudentRecord> studentList in SplitPackage<SmartSchool.Customization.Data.StudentRecord>(GetList<SmartSchool.Customization.Data.StudentRecord>(e.List)))
            {
                Dictionary<string, List<SmartSchool.Customization.Data.StudentExtension.UpdateRecordInfo>> studentUpdateRecords = new Dictionary<string, List<SmartSchool.Customization.Data.StudentExtension.UpdateRecordInfo>>();
                //���o�s��
                #region ���o�s��
                string[] idList = new string[studentList.Count];
                for (int i = 0; i < idList.Length; i++)
                {
                    idList[i] = studentList[i].StudentID;
                }
                if (idList.Length == 0)
                    continue;
                #endregion
                //�즨�Z���
                #region �즨�Z���
                foreach (XmlElement element in SmartSchool.Feature.QueryStudent.GetUpdateRecordByStudentIDList(idList).GetContent().GetElements("UpdateRecord"))
                {
                    string RefStudentID = element.GetAttribute("RefStudentID");
                    if (!studentUpdateRecords.ContainsKey(RefStudentID))
                        studentUpdateRecords.Add(RefStudentID, new List<SmartSchool.Customization.Data.StudentExtension.UpdateRecordInfo>());
                    studentUpdateRecords[RefStudentID].Add(new UpdateRecord(updateCodeMappingElement, element));
                }
                #endregion
                //��J�ǥͪ����ʸ�ƲM��
                #region ��J�ǥͪ����ʸ�ƲM��
                foreach (SmartSchool.Customization.Data.StudentRecord student in studentList)
                {
                    student.UpdateRecordList.Clear();
                    if (studentUpdateRecords.ContainsKey(student.StudentID))
                    {
                        foreach (SmartSchool.Customization.Data.StudentExtension.UpdateRecordInfo updateRecord in studentUpdateRecords[student.StudentID])
                        {
                            student.UpdateRecordList.Add(updateRecord);
                        }
                    }
                }
                #endregion
            }
        }
    }
}
