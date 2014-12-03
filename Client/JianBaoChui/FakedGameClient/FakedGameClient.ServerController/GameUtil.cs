using System;
using System.IO;
using System.Net;
using System.Text;
namespace FakedGameClient
{
    public class GameUtil
    {
        public static string GetGestureName(GuessGesture g)
        {
            string gesture = string.Empty;
            switch (g)
            {
                case GuessGesture.Bao:
                    gesture = "包子";
                    break;
                case GuessGesture.Jian:
                    gesture = "剪子";
                    break;
                case GuessGesture.Chui:
                    gesture = "锤子";
                    break;
                default:
                    gesture = "未知";
                    break;
            }
            return gesture;
        }

        //1:g1胜 2:g2胜 0:平
        public static int Guess(GuessGesture g1, GuessGesture g2)
        {
            int result = g1 - g2;
            if (-1 == result || 2 == result)
            {
                return 1;
            }
            else if (0 == result)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public static string GetGameStatusName(GameStatus s)
        {
            string status = string.Empty;
            switch (s)
            {
                case GameStatus.Final:
                    status = "决赛";
                    break;
                case GameStatus.Final4:
                    status = "4强赛";
                    break;
                case GameStatus.Final8:
                    status = "8强赛";
                    break;
                case GameStatus.PK:
                    status = "PK赛";
                    break;
                case GameStatus.Qualifier:
                    status = "预选赛";
                    break;
                default:
                    status = "未知";
                    break;
            }
            return status;
        }

        public static string GetGameStateName(GameState gs)
        {
            string state = string.Empty;
            switch (gs)
            {
                case GameState.KOStart:
                    state = "KO开赛"; break;
                case GameState.KO:
                    state = "KO赛中"; break;
                case GameState.KOEnd:
                    state = "KO赛终"; break;
                case GameState.PKStart:
                    state = "PK开赛"; break;
                case GameState.PK:
                    state = "PK赛中"; break;
                case GameState.PKEnd:
                    state = "PK赛终"; break;
                default:
                    state = "未知"; break;
            }
            return state;
        }

        public static string GetPlayerStateName(PlayerState ps)
        {
            string state = string.Empty;
            switch (ps)
            {
                case PlayerState.Connected:
                    state = "已连接"; break;
                case PlayerState.Signed:
                    state = "已报名"; break;
                case PlayerState.Watching:
                    state = "观战"; break;
                case PlayerState.RoundStart:
                    state = "回合开始"; break;
                case PlayerState.RoundEnd:
                    state = "回合结束"; break;
                default:
                    state = "未知"; break;
            }
            return state;
        }

        public static string GetRoundResultName(RoundResult rr)
        {
            string result = string.Empty;
            switch (rr)
            {
                case RoundResult.Win:
                    result = "获胜";
                    break;
                case RoundResult.Lose:
                    result = "失利";
                    break;
                case RoundResult.Draw:
                    result = "不分胜负";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        public static string GetPlayerStatusName(PlayerStatus ps)
        {
            string result = string.Empty;
            switch (ps)
            {
                case PlayerStatus.Defier:
                    result = "未知";
                    break;
                case PlayerStatus.Winner:
                    result = "擂主";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        //获得[1~max]范围内随机数,包含边界
        public static int Random(int max)
        {
            return DateTime.Now.TimeOfDay.Milliseconds % max + 1;
        }

        /// <summary>
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)
        /// </summary>
        /// <param name="address">文件上传到的服务器</param>
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <param name="progressBar">上传进度条</param>
        /// <returns>成功返回1，失败返回0</returns>
        public static int UploadRequest(string address, string fileNamePath, string saveName, int type, System.Windows.Forms.ProgressBar progressBar1)
        {
            int returnValue = 0;
            // 要上传的文件
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            //时间戳
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
            //请求头部信息
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; ");
            if (type == 1)
            {
                sb.Append("name=\"");
                sb.Append("textarea");
            }
            else if (type == 2)
            {
                sb.Append("name=\"");
                sb.Append("file");
                sb.Append("\"; filename=\"");
                sb.Append(saveName);
            }
            else if (type == 3)
            {
                sb.Append("name=\"");
                sb.Append("point");
                sb.Append("\"; filename=\"");
                sb.Append(saveName);
            }

            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
            // 根据uri创建HttpWebRequest对象
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";
            //对发送的数据不使用缓存
            httpReq.AllowWriteStreamBuffering = false;
            //设置获得响应的超时时间（300秒）
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            httpReq.Referer = "localhost";
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            long fileLength = fs.Length;
            httpReq.ContentLength = length;
            try
            {
                //lblProcess.Text = "正在上传配置,请稍后...";

                progressBar1.Maximum = int.MaxValue;
                progressBar1.Minimum = 0;
                progressBar1.Value = 0;
                //每次上传4k
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];
                //已上传的字节数
                long offset = 0;
                //开始上传时间
                DateTime startTime = DateTime.Now;
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();
                //发送请求头部消息
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    progressBar1.Value = (int)(offset * (int.MaxValue / length));
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    //lblProcess.Text = "已用时：" + second.ToString("F2") + "秒";
                    //if (second > 0.001)
                    //{
                    //lblProcess.Text += "    平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒";
                    //}
                    //else
                    //{
                    //lblProcess.Text += "    正在连接…";
                    //}
                    //lblProcess.Text += "    已上传：" + (offset * 100.0 / length).ToString("F2") + "%";
                    //lblProcess.Text += "    " + (offset / 1048576.0).ToString("F2") + "M/" + (fileLength / 1048576.0).ToString("F2") + "M";
                    //Application.DoEvents();
                    size = r.Read(buffer, 0, bufferLength);
                    if (size == 0)
                    {
                        progressBar1.Value = progressBar1.Maximum;
                    }
                }
                //添加尾部的时间戳
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();
                //获取服务器端的响应
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                //读取服务器端返回的消息
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                if (sReturnString == "Success")
                {
                    returnValue = 1;
                }
                else if (sReturnString == "Failed")
                {
                    returnValue = 0;
                }
            }
            catch (Exception ex)
            {
                returnValue = 0;
                throw ex;
            }
            finally
            {
                fs.Close();
                r.Close();
            }
            return returnValue;
        }
    }
}