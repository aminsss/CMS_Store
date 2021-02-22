using System.Net;
using System.IO;
using System.Configuration;
using System;

public class FtpHelper
{
    public FtpHelper()
    {
        //Default Value Set From Application
        //_hostname = ConfigurationManager.AppSettings.GetValues("FtpAddress")[0];
        //_username = ConfigurationManager.AppSettings.GetValues("FtpUser")[0];
        //_password = ConfigurationManager.AppSettings.GetValues("FtpPass")[0];
        _hostname = "statics-kspub.koshop.ir/public_html/";
        _username = "staticsk";
        _password = "26:BDqoOn9nC2!";
    }

    #region "Properties"
    private string _hostname;
    /// <summary>
    /// Hostname
    /// </summary>
    /// <value></value>
    /// <remarks>Hostname can be in either the full URL format
    /// ftp://ftp.myhost.com or just ftp.myhost.com
    /// </remarks>
    public string Hostname
    {
        get
        {
            if (_hostname.StartsWith("ftp://"))
            {
                return _hostname;
            }
            else
            {
                return "ftp://" + _hostname;
            }
        }
        set
        {
            _hostname = value;
        }
    }
    private string _username;
    /// <summary>
    /// Username property
    /// </summary>
    /// <value></value>
    /// <remarks>Can be left blank, in which case 'anonymous' is returned</remarks>
    public string Username
    {
        get
        {
            return (_username == "" ? "anonymous" : _username);
        }
        set
        {
            _username = value;
        }
    }
    private string _password;
    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            _password = value;
        }
    }


    #endregion

    public static bool Upload(string fileUrl,string folder)
    {
        if (File.Exists(fileUrl))
        {
            FtpHelper ftpClient = new FtpHelper();
            string ftpUrl = ftpClient.Hostname + folder + System.IO.Path.GetFileName(fileUrl);


            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);
            ftp.Credentials = new NetworkCredential(ftpClient.Username, ftpClient.Password);

            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Timeout = 3600000;
            ftp.KeepAlive = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;

            const int bufferLength = 102400;
            byte[] buffer = new byte[bufferLength];
            int readBytes = 0;

            //open file for reading
            using (FileStream fs = File.OpenRead(fileUrl))
            {
                //try
                {
                    //open request to send
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        do
                        {
                            readBytes = fs.Read(buffer, 0, bufferLength);
                            rs.Write(buffer, 0, readBytes);
                        } while (!(readBytes < bufferLength));
                        rs.Close();
                    }

                }
                //catch (Exception)
                {
                    //Optional Alert for Exeption To Application Layer
                    //throw (new ApplicationException("بارگذاری فایل با خطا  رو به رو شد"));

                }
                //finally
                {
                    //ensure file closed
                    //fs.Close();
                }

            }

            ftp = null;
            return true;
        }
        return false;



    }
}

