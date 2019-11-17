using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace WebApplicationCRUD
{
    public partial class CRUDTable : System.Web.UI.Page
    {
        private SqlConnection conn;
        private SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strConn = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            conn = new SqlConnection(strConn);

            if (!IsPostBack)
            {
                bind(null, null);

                //add attribute textbox
                TxtNilai.Attributes.Add("onkeyup", "javascript:grade()");
                TxtNilai.Attributes.Add("onkeydown", "javascript:grade()");

                //btnUpdate.Enabled = false;
                //btnDelete.Enabled = false;
                //btnUpdate.Style.Add("class", "button disable"); //menambah class css
                //btnDelete.Style.Add("class", "button disable");

                btnUpdate.Attributes["class"] = "button disabled"; //modifikasi class css
                btnDelete.Attributes["class"] = "button disabled";
            }
        }

        protected void btnSimpan_Click(object sender, EventArgs e)
        {
            string sql = @"insert into SubjectDetails (SubjectName, Marks, Grade) values (@SubjectName, @Marks, @Grade)";

            cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@SubjectName", TxtSubjectName.Text);
            cmd.Parameters.AddWithValue("@Marks", Convert.ToInt32(TxtNilai.Text));
            cmd.Parameters.AddWithValue("@Grade", TxtGrade.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                ClientScript.RegisterStartupScript(this.GetType(),
                                                    "alert",
                                                    "alert('" + TxtSubjectName.Text + " Berhasil ditambahkan!');", true);

                bind(null, null);
                btnCancel_Click(null, null);
            }
            catch (SqlException sqlEx)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                                                   "alert",
                                                   "alert('Pesan : " + sqlEx.Message + "');", true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string sql = @"update SubjectDetails set SubjectName = @SubjectName, Marks = @Marks, Grade = @Grade 
                            where SubjectId = @SubjectId";

            cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@SubjectID", TxtID.Text);
            cmd.Parameters.AddWithValue("@SubjectName", TxtSubjectName.Text);
            cmd.Parameters.AddWithValue("@Marks", Convert.ToInt32(TxtNilai.Text));
            cmd.Parameters.AddWithValue("@Grade", TxtGrade.Text);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                ClientScript.RegisterStartupScript(this.GetType(),
                                                    "alert",
                                                    "alert('" + TxtSubjectName.Text + " Berhasil ditambahkan!');", true);

                bind(null, null);
                btnCancel_Click(null, null);
            }
            catch (SqlException sqlEx)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                                                   "alert",
                                                   "alert('Pesan : " + sqlEx.Message + "');", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Debug.WriteLine(TxtID.Text);
            string sql = "delete SubjectDetails where subjectid = " + Int32.Parse(TxtID.Text);
            Debug.WriteLine(sql);

            conn.Open();
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            btnCancel_Click(null, null);
            bind(null, null);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            resetAll();
            //btnSimpan.Enabled = true;
            //btnUpdate.Enabled = false;
            //btnDelete.Enabled = false;

            btnUpdate.Attributes["class"] = "button disabled"; //modifikasi class css
            btnDelete.Attributes["class"] = "button disabled";
            btnSimpan.Attributes["class"] = "button";
        }

        private void resetAll()
        {
            TxtID.Text = "";
            TxtSubjectName.Text = "";
            TxtNilai.Text = "";
            TxtGrade.Text = "";
        }

        private void bind(String sortColumn, String sortDirection)
        {
            String sql;

            if (sortColumn == null && sortDirection == null)
            {
                sql = "SELECT * FROM SubjectDetails";
            }
            else
            {
                sortDirection = (sortDirection == "Ascending") ? "ASC" : "DESC";
                sql = "SELECT * FROM SubjectDetails order by " + sortColumn + " " + sortDirection;
            }

            cmd = new SqlCommand(sql, conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
                conn.Close();

                LbData.Text = "Data: " + dt.Rows.Count.ToString();
            }
            catch (SqlException sqlEx)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                                                   "alert",
                                                   "alert('Pesan : " + sqlEx.Message + "');", true);

            }

        }

        //on row command object "button field" tak bisa on client script dan command argument
        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "cmPilih")
            {
                /* Ada 2 nilai pada command argument
                 * 1. index row
                 * 2. ID Sibject dari tabel                 
                 */

                string val = e.CommandArgument.ToString();
                string[] idx = val.Split('-');

                //Debug.Write("text: " + val);
                //Debug.Write("index: " + idx[0].ToString() + " | " + idx[1].ToString());                
                //GridViewRow row = GridView1.Rows[0]; //GridView Row.    

                GridViewRow row = GridView1.Rows[Convert.ToInt32(idx[0])]; //GridView Row.    

                //TxtID.Text = e.CommandArgument.ToString();
                TxtID.Text = idx[1].ToString();
                TxtSubjectName.Text = row.Cells[1].Text;
                TxtNilai.Text = row.Cells[2].Text;
                TxtGrade.Text = row.Cells[3].Text;

                btnUpdate.Attributes["class"] = "button"; //modifikasi class css
                btnDelete.Attributes["class"] = "button alert";
                btnSimpan.Attributes["class"] = "button disabled";

            }
            else if (e.CommandName == "cmHapus")
            {
                string sql = "delete SubjectDetails where subjectid = " + e.CommandArgument;

                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                resetAll();
                bind(null, null);

                btnUpdate.Attributes["class"] = "button disabled"; //modifikasi class css
                btnDelete.Attributes["class"] = "button disabled";
                btnSimpan.Attributes["class"] = "button";
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            bind(Convert.ToString(e.SortExpression), Convert.ToString(e.SortDirection));
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            bind(null, null);
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }
    }
}