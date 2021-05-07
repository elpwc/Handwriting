using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace HandWriting_output
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MailAddress from = new MailAddress("1014193759@qq.com", "Hangwriting的用户"); //邮件的发件人

            MailMessage mail = new MailMessage();

            //设置邮件的标题
            mail.Subject = "来自hangwriting用户的反馈";

            //设置邮件的发件人
            //Pass:如果不想显示自己的邮箱地址，这里可以填符合mail格式的任意名称，真正发mail的用户不在这里设定，这个仅仅只做显示用
            mail.From = from;

            //设置邮件的收件人
            //string address = "elpwc@hotmail.com";
            //string displayName = "";
            /*  这里这样写是因为可能发给多个联系人，每个地址用 ; 号隔开
              一般从地址簿中直接选择联系人的时候格式都会是 ：用户名1 < mail1 >; 用户名2 < mail 2>; 
              因此就有了下面一段逻辑不太好的代码
              如果永远都只需要发给一个收件人那么就简单了 mail.To.Add("收件人mail");
            */
            //string[] mailNames = (txtMailTo.Text + ";").Split(';');
            //foreach (string name in mailNames)
            //{
            //    if (name != string.Empty)
            //    {
            //        if (name.IndexOf('<') > 0)
            //        {
            //            displayName = name.Substring(0, name.IndexOf('<'));
            //            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
            //        }
            //        else
            //        {
            //            displayName = string.Empty;
            //            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
            //        }
            //        mail.To.Add(new MailAddress(address, displayName));
            //    }
            //}
            mail.To.Add("elpwc@hotmail.com");
            //设置邮件的抄送收件人
            //这个就简单多了，如果不想快点下岗重要文件还是CC一份给领导比较好
            //mail.CC.Add(new MailAddress("Manage@hotmail.com", "尊敬的领导");

            //设置邮件的内容
            mail.Body = textBox1.Text+"\r\n\r\n\r\n联系方式："+textBox2.Text;
            //设置邮件的格式
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            //设置邮件的发送级别
            mail.Priority = MailPriority.Normal;

            //设置邮件的附件，将在客户端选择的附件先上传到服务器保存一个，然后加入到mail中
            //string fileName = txtUpFile.PostedFile.FileName.Trim();
            //fileName = "D:/UpFile/" + fileName.Substring(fileName.LastIndexOf("/") + 1);
            //txtUpFile.PostedFile.SaveAs(fileName); // 将文件保存至服务器
            //mail.Attachments.Add(new Attachment(fileName));

            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            SmtpClient client = new SmtpClient();
            //设置用于 SMTP 事务的主机的名称，填IP地址也可以了
            client.Host = "smtp.qq.com";
            client.EnableSsl = true;
            //设置用于 SMTP 事务的端口，默认的是 25
            //client.Port = 25;
            client.UseDefaultCredentials = false;
            //这里才是真正的邮箱登陆名和密码，比如我的邮箱地址是 hbgx@hotmail， 我的用户名为 hbgx ，我的密码是 xgbh

            client.Credentials = new System.Net.NetworkCredential("1014193759", "ovehafatdybjbcec");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //都定义完了，正式发送了，很是简单吧！
            bool failed = false;
            try
            {
                client.Send(mail);

            }
            catch (Exception)
            {
                failed = true;
                MessageBox.Show("发送失败..");
            }
            if(!failed)
            {
                MessageBox.Show("发送成功！\r\n之后会回复你的反馈哦！");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Height = 670;
        }
    }
}
