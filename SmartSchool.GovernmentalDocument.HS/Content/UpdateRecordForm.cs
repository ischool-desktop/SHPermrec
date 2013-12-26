using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Controls;
using FISCA.Permission;
using FISCA.Presentation.Controls;
using SHSchool.Data;

namespace SmartSchool.GovernmentalDocument.Content
{
    public partial class UpdateRecordForm : FISCA.Presentation.Controls.BaseForm
    {
        //�ǥͽs��
        private string _studentid;
        private string _updateid;
        //���ʰO���s��
        public event EventHandler DataSaved;
        private bool _saved;

        private Dictionary<UpdateRecordType, XmlElement> _tempInfo;
        private UpdateRecordType _previousType;

        public UpdateRecordForm(string id, string updateid)
        {
            _studentid = id;
            _updateid = updateid;
            _saved = false;
            _tempInfo = new Dictionary<UpdateRecordType, XmlElement>();
            _tempInfo.Add(UpdateRecordType.���~����, null);
            _tempInfo.Add(UpdateRecordType.�s�Ͳ���, null);
            _tempInfo.Add(UpdateRecordType.���y����, null);
            _tempInfo.Add(UpdateRecordType.��J����, null);
            InitializeComponent();
            Initialize();

            FeatureAce ace = FISCA.Permission.UserAcl.Current[UpdatePalmerwormItem.FeatureCode];

            btnOK.Visible = ace.Editable;

            if (!ace.Editable)
                LockAllControl(this);
        }

        private void LockAllControl(Control parent)
        {
            foreach (Control each in parent.Controls)
            {
                if (each is TextBoxX)
                    (each as TextBoxX).ReadOnly = true;

                if (each is ComboBoxEx)
                    (each as ComboBoxEx).Enabled = false;

                if (each.Controls.Count > 0)
                    LockAllControl(each);
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //����ثe����Ʀs�_��          
            _tempInfo[_previousType] = updateRecordInfo1.GetElement();


            switch (comboBoxEx1.SelectedIndex)
            {
                default:
                case 0:
                    updateRecordInfo1.Style = UpdateRecordType.���y����;
                    break;
                case 1:
                    updateRecordInfo1.Style = UpdateRecordType.��J����;
                    break;
                case 2:
                    updateRecordInfo1.Style = UpdateRecordType.�s�Ͳ���;
                    break;
                case 3:
                    updateRecordInfo1.Style = UpdateRecordType.���~����;
                    break;
            }

            XmlElement typeRec = _tempInfo[updateRecordInfo1.Style];
            if (typeRec != null)
                BindDataFromElement(typeRec);

            _previousType = updateRecordInfo1.Style;
        }

        private void Initialize()
        {
            if (!string.IsNullOrEmpty(_updateid))
            {
                updateRecordInfo1.StudentID = _studentid;
                updateRecordInfo1.SetUpdateValue(_updateid);
            }
            else
            {
                updateRecordInfo1.SetDefaultValue(_studentid);
            }
            switch (updateRecordInfo1.Style)
            {
                case UpdateRecordType.���y����:
                    comboBoxEx1.SelectedIndex = 0;
                    break;
                case UpdateRecordType.��J����:
                    comboBoxEx1.SelectedIndex = 1;

                    break;
                case UpdateRecordType.�s�Ͳ���:
                    comboBoxEx1.SelectedIndex = 2;

                    break;
                case UpdateRecordType.���~����:
                    comboBoxEx1.SelectedIndex = 3;
                    break;
                default:
                    break;
            }
            _previousType = updateRecordInfo1.Style;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // �ˬd�O�_�n�ק�ǥͪ��A
            int UpdateCoodeInt;
            bool UpdateStudStatus = false;

            if (int.TryParse(updateRecordInfo1.UpdateCode, out UpdateCoodeInt))
            {
                string StudIDNumber = "", StudNumber = "";

                SHStudentRecord stud = SHStudent.SelectByID(updateRecordInfo1.StudentID);

                if (stud != null)
                {
                    StudIDNumber = stud.IDNumber;
                    StudNumber = stud.StudentNumber;
                }


                // �����ˬd�θ��
                // �@��
                List<string> tmp01 = new List<string>();
                // ���~������
                List<string> tmp02 = new List<string>();
                // ���
                List<string> tmp03 = new List<string>();

                foreach (SHStudentRecord studRec in SHStudent.SelectAll())
                {
                    if (studRec.Status == K12.Data.StudentRecord.StudentStatus.�@��)
                    {
                        tmp01.Add(studRec.IDNumber);
                        tmp01.Add(studRec.StudentNumber);
                    }

                    if (studRec.Status == K12.Data.StudentRecord.StudentStatus.���~������)
                    {
                        tmp02.Add(studRec.IDNumber);
                        tmp02.Add(studRec.StudentNumber);
                    }

                    if (studRec.Status == K12.Data.StudentRecord.StudentStatus.���)
                    {
                        tmp03.Add(studRec.IDNumber);
                        tmp03.Add(studRec.StudentNumber);
                    }
                }

                // �_��
                if (UpdateCoodeInt >= 221 && UpdateCoodeInt <= 226)
                {
                    if (MessageBox.Show("�аݬO�_���ǥͪ��A�� �@��H", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // �ˬd�Ӫ��A�O�_���ۦP�Ǹ��Ψ����Ҹ��ǥ�
                        if (tmp01.Contains(StudIDNumber) || tmp01.Contains(StudNumber))
                        {
                            FISCA.Presentation.Controls.MsgBox.Show("�b�@�몬�A�w���ۦP�����Ҹ��ξǸ����ǥ͡A�L�k�۰��ܧ󪬺A�C");
                        }
                        else
                        {
                            UpdateStudStatus = true;
                            stud.Status = K12.Data.StudentRecord.StudentStatus.�@��;
                        }
                    }
                }

                // ��X
                if (UpdateCoodeInt >= 311 && UpdateCoodeInt <= 316)
                {
                    if (MessageBox.Show("�аݬO�_���ǥͪ��A�� ���~�����աH", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // �ˬd�Ӫ��A�O�_���ۦP�Ǹ��Ψ����Ҹ��ǥ�
                        if (tmp02.Contains(StudIDNumber) || tmp02.Contains(StudNumber))
                        {
                            FISCA.Presentation.Controls.MsgBox.Show("�b���~�����ժ��A�w���ۦP�����Ҹ��ξǸ����ǥ͡A�L�k�۰��ܧ󪬺A�C");
                        }
                        else
                        {
                            UpdateStudStatus = true;
                            stud.Status = K12.Data.StudentRecord.StudentStatus.���~������;
                        }
                    }
                }

                // ���
                if (UpdateCoodeInt >= 341 && UpdateCoodeInt <= 349)
                {
                    if (MessageBox.Show("�аݬO�_���ǥͪ��A�� ��ǡH", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("�аݬO�_���ǥͪ��A�� ���~�����աH", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            // �ˬd�Ӫ��A�O�_���ۦP�Ǹ��Ψ����Ҹ��ǥ�
                            if (tmp03.Contains(StudIDNumber) || tmp03.Contains(StudNumber))
                            {
                                FISCA.Presentation.Controls.MsgBox.Show("�b��Ǫ��A�w���ۦP�����Ҹ��ξǸ����ǥ͡A�L�k�۰��ܧ󪬺A�C");
                            }
                            else
                            {
                                UpdateStudStatus = true;
                                stud.Status = K12.Data.StudentRecord.StudentStatus.���;
                            }
                        }
                    }
                }
                // ��s�ǥͪ��A
                if (UpdateStudStatus)
                {
                    SHStudent.Update(stud);
                    StudentRelated.Student.Instance.SyncAllBackground();
                }
            }
            if (updateRecordInfo1.Save())
            {
                _saved = true;
                if (DataSaved != null)
                    DataSaved(this, null);
                this.Close();
            }
        }

        private void UpdateRecordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CurrentUser.Acl[UpdatePalmerwormItem.FeatureCode].Editable)
                return;

            if (!_saved)
            {
                if (MsgBox.Show("�o�Ӱʧ@�N���ثe�s�褤����ơA�O�_�T�w���}?", "����", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// ���Ҹ�ƬO�_���~
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            bool valid = updateRecordInfo1.IsValid();
            if (!valid)
                MsgBox.Show("��ƿ��~�A���ˬd��J���", "���~", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return valid;
        }

        private string GetElementValue(XmlElement element, string xpath)
        {
            if (element == null) return "";
            if (element.SelectSingleNode(xpath) == null)
                return "";
            return element.SelectSingleNode(xpath).InnerText;
        }

        private void BindDataFromElement(XmlElement element)
        {
            updateRecordInfo1.Name = GetElementValue(element, "Name");
            updateRecordInfo1.StudentNumber = GetElementValue(element, "StudentNumber");
            updateRecordInfo1.IDNumber = GetElementValue(element, "IDNumber");
            updateRecordInfo1.Gender = GetElementValue(element, "Gender");
            updateRecordInfo1.Birthdate = GetElementValue(element, "Birthdate");
            updateRecordInfo1.UpdateDate = GetElementValue(element, "UpdateDate");
            updateRecordInfo1.UpdateCode = GetElementValue(element, "UpdateCode");
            updateRecordInfo1.UpdateDescription = GetElementValue(element, "UpdateDescription");
            updateRecordInfo1.Comment = GetElementValue(element, "Comment");
            updateRecordInfo1.GradeYear = GetElementValue(element, "GradeYear");
            updateRecordInfo1.ADDate = GetElementValue(element, "ADDate");
            updateRecordInfo1.ADNumber = GetElementValue(element, "ADNumber");
            updateRecordInfo1.Department = GetElementValue(element, "Department");
            updateRecordInfo1.LastADDate = GetElementValue(element, "LastADDate");
            updateRecordInfo1.LastADNumber = GetElementValue(element, "LastADNumber");
            updateRecordInfo1.LastUpdateCode = GetElementValue(element, "LastUpdateCode");
            updateRecordInfo1.PreviousDepartment = GetElementValue(element, "PreviousDepartment");
            updateRecordInfo1.PreviousGradeYear = GetElementValue(element, "PreviousGradeYear");
            updateRecordInfo1.PreviousSchool = GetElementValue(element, "PreviousSchool");
            updateRecordInfo1.PreviousSchoolLastADDate = GetElementValue(element, "PreviousSchoolLastADDate");
            updateRecordInfo1.PreviousSchoolLastADNumber = GetElementValue(element, "PreviousSchoolLastADNumber");
            updateRecordInfo1.PreviousStudentNumber = GetElementValue(element, "PreviousStudentNumber");
            updateRecordInfo1.GraduateCertificateNumber = GetElementValue(element, "GraduateCertificateNumber");
            updateRecordInfo1.GraduateSchool = GetElementValue(element, "GraduateSchool");
            updateRecordInfo1.GraduateSchoolLocationCode = GetElementValue(element, "GraduateSchoolLocationCode");            
            
            #region 2009�~�s��s�W
            updateRecordInfo1.ClassType = GetElementValue(element,"ClassType");
            updateRecordInfo1.SpecialStatus = GetElementValue(element,"SpecialStatus");
            updateRecordInfo1.IDNumberComment = GetElementValue(element,"IDNumberComment");
            updateRecordInfo1.OldClassType = GetElementValue(element,"OldClassType");
            updateRecordInfo1.OldDepartmentCode = GetElementValue(element,"OldDepartmentCode");
            updateRecordInfo1.GraduateSchoolYear = GetElementValue(element,"GraduateSchoolYear");
            updateRecordInfo1.GraduateComment = GetElementValue(element,"GraduateComment");
            #endregion 
        }

        /// <summary>
        /// �P�_�ثe��ƬO�ݩ��ز������O��
        /// ���]���ثe��Ƥ����A�ҥH�٤��T�w�n���P�_
        /// ���g���@��function , �N�û��Ǧ^ "���y����";
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>�������O</returns>
        private UpdateRecordType GetUpdateRecordType(object arg)
        {
            return UpdateRecordType.���y����;
        }
    }
}