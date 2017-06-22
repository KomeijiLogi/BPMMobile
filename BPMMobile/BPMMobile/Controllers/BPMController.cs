using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Helper;
using BPM.Client;
using BPM;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Data;
using BPMMobile.Models;
using System.Configuration;
using YUN;
using log4net;
using System.Reflection;


namespace BPMMobile.Controllers
{
    [YunAuthenticate]
    public class BPMController : ApiController
    {
        [HttpPost]
        //[Route("PostAccount")] 
        public IHttpActionResult PostAccount([FromBody] PostAccountInput input)
        {
            string yunxt = ConfigurationManager.AppSettings["yunxt"];//轻应用注册到云之家时生成 
            string eid = ConfigurationManager.AppSettings["eid"];// 
            string keyFile = ConfigurationManager.AppSettings["keyFile"];// 
            string privatekey = AppDomain.CurrentDomain.BaseDirectory + keyFile;
            byte[] data;
            using (FileStream fs = File.OpenRead(privatekey))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
            }
            var resp = YUNAPI.GetPersonInfo(yunxt, eid, data, input.ids);
            return Json(resp);

        }



        /// <summary>
        /// 可发起流程列表
        /// </summary>
        /// <returns></returns>

        public IHttpActionResult GetCanCreateProcess()
        {
            PrepareBPMEnv();

            List<Process> myProcesses = new List<Process> ();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
               
                BPMProcessCollection processes = new BPMProcessCollection();
                var mobileProcess = GetMobileProcess();
                var cache = new Dictionary<string, BPMProcessCollection>();
                foreach (var p in mobileProcess)
                {
                    if (!p.CanCreate) continue;
                    if (cache.ContainsKey(p.Path))
                    {
                        processes = cache[p.Path];
                    }
                    else
                    {
                        processes = cn.GetProcessList(p.Path, BPMPermision.Execute, true);
                        cache.Add(p.Path, processes);
                    }
                    foreach (BPMProcess item in processes)
                    {
                        if (item.Name == p.Name)
                        {
                            myProcesses.Add(p);
                        }
                    }
                }
            }
                return Json(myProcesses);
            }
        /// <summary>
        /// 获取待办任务数量
        /// </summary>
        /// <returns></returns>
        public int GetMyTaskCount()
        {
            PrepareBPMEnv();
            var tasks= GetMyTaskList();
            return tasks.Count;
        }
      
        /// <summary>
        /// 获取全部待办任务
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetMyUndoTaskAll()
        {
            var taskList=GetMyTaskList();
            var mobileProcess = GetMobileProcess();
            var ret = from t in taskList
                        join m in mobileProcess on t.ProcessName equals m.Name
                        select new TaskDto()
                        {
                            DisplayName = m.DisplayName,
                            Icon=m.Icon,
                            ReceiveAt=t.ReceiveAt,
                            CreateAt=t.CreateAt,
                            ViewUndoPage = m.ViewUndoPage,
                            ViewAskPage = m.ViewAskPage,
                            ViewDonePage = m.ViewDonePage,
                            Description =t.Description,
                            ProcessName=t.ProcessName,
                            StepID=t.StepID,
                            StepName=t.StepName,
                            TaskID=t.TaskID
                        };
    
            return Json(ret);
        }

        


        /// <summary>
        /// 获取待办任务分页
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetMyUndoTask(int startRowIndex,int rows)
        {
            var allList = GetMyTaskList();
            var taskList = allList
                .OrderByDescending(t=>t.ReceiveAt)
                .Skip(startRowIndex)
                .Take(rows);
            var mobileProcess = GetMobileProcess();
            var ret = from t in taskList
                      join m in mobileProcess on t.ProcessName equals m.Name
                      select new TaskDto()
                      {
                          DisplayName = m.DisplayName,
                          Icon = m.Icon,
                          ReceiveAt = t.ReceiveAt,
                          ViewUndoPage = m.ViewUndoPage,
                          ViewAskPage=m.ViewAskPage,
                          ViewDonePage=m.ViewDonePage,
                          Description = t.Description,
                          ProcessName = t.ProcessName,
                          StepID = t.StepID,
                          StepName = t.StepName,
                          TaskID = t.TaskID
                      };
            return   Json(new { tasks=ret,total= allList.Count}); 
        }
        /// <summary>
        /// 我发起的
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IHttpActionResult GetMyRequest(int startRowIndex, int rows)
        {
            var allList = GetMyRequestList();
            var taskList = allList
                .OrderByDescending(t=>t.CreateAt)
                .Skip(startRowIndex)
                .Take(rows);
            var mobileProcess = GetMobileProcess();
            var ret = from t in taskList
                      join m in mobileProcess on t.ProcessName equals m.Name
                      select new TaskDto()
                      {
                          DisplayName = m.DisplayName,
                          Icon = m.Icon,
                          CreateAt = t.CreateAt,
                          ViewUndoPage = m.ViewUndoPage,
                          ViewAskPage=m.ViewAskPage,
                          ViewDonePage=m.ViewDonePage,
                          Description = t.Description,
                          ProcessName = t.ProcessName,
                          TaskState =t.TaskState,
                          TaskID = t.TaskID
                      };
            return Json(new { tasks = ret, total = allList.Count });

        }
       
        /// <summary>
        /// 已办的
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IHttpActionResult GetMyProcessed(int startRowIndex, int rows)
        {
            var allList = GetMyProcessedList();
            var taskList = allList
                .OrderByDescending(t => t.CreateAt)
                .Skip(startRowIndex)
                .Take(rows);
            var mobileProcess = GetMobileProcess();
            var ret = from t in taskList
                      join m in mobileProcess on t.ProcessName equals m.Name
                      select new TaskDto()
                      {
                          DisplayName = m.DisplayName,
                          Icon = m.Icon,
                          CreateAt = t.CreateAt,
                          ViewUndoPage = m.ViewUndoPage,
                          ViewAskPage = m.ViewAskPage,
                          ViewDonePage = m.ViewDonePage,
                          Description = t.Description,
                          ProcessName = t.ProcessName,
                          TaskID = t.TaskID
                      };
            return Json(new { tasks = ret, total = allList.Count });

        }
        /// <summary>
        /// 发起 审批 加签 流程
        /// </summary>
        /// <param name=""></param>
        public IHttpActionResult PostProcess([FromBody]string xml )
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            try {
                PrepareBPMEnv();
                using (BPMConnection cn = new BPMConnection())
                {
                    cn.WebOpen();
                    byte[] bs = System.Text.Encoding.UTF8.GetBytes(xml);
                    MemoryStream xlmStream = new MemoryStream(bs);
                    PostResult result = BPMProcess.Post(cn, xlmStream);
                    return Json(result);
                }

            } catch (Exception e) {

                log.Error("error:"+e);
            }
            return null;
        }
        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostRecedeRestart(PostRecedeRestartModel model)
        {
            PrepareBPMEnv();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                User result = BPMTask.RecedeRestart(cn, model.taskID, model.comments);
                return Json(result);
            }
        }

        /// <summary>
        /// 退回某步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostRecedeBack(PostRecedeBackModel model)
        {
            PrepareBPMEnv();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                BPMStepCollection newSteps= BPMProcStep.RecedeBack(cn, model.stepid, model.toStepIDs, model.comments);
                List<string> to = new List<string>();
                foreach (BPMProcStep step in newSteps)
                    to.Add(String.Format("{0}[{1}]", step.NodeName, YZStringHelper.GetUserFriendlyName(step.RecipientAccount, step.RecipientFullName)));

                JsonItem rv = new JsonItem();
                rv.Attributes.Add("success", true);
                rv.Attributes.Add("tosteps", String.Join(";", to.ToArray()));
                return Json(rv.ToString());
            }
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostReject(PostRecedeRestartModel model)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            PrepareBPMEnv();
            try {
                using (BPMConnection cn = new BPMConnection())
                {
                    cn.WebOpen();
                    BPMTask.Reject(cn, model.taskID, model.comments);
                    return Json("ok");
                }
            } catch (Exception e) {
                log.Error("error:", e);
            }
            return null;

        }

        /// <summary>
        /// 知会
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostInform(PostInformModel inform)
        {
            var bpmAccounts = new BPMObjectNameCollection();
            
            
            bpmAccounts.AddRange(inform.accounts);
            PrepareBPMEnv();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                var result = BPMTask.Inform(cn, inform.taskID, bpmAccounts, inform.comments);
                return Json(result);
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadFiles()
        {
            HttpFileCollection files;
           
            ILog log= LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            

            try {
               
           
                files = System.Web.HttpContext.Current.Request.Files;
                List<Attachment> attachments = new List<Attachment>();

                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].ContentLength > 0)
                    {
                        HttpPostedFile file = files[i];
                        string fileName = Path.GetFileName(file.FileName);
                        long fileSize = file.ContentLength;
                        string fileExt = Path.GetExtension(fileName).ToLower();
                        string fileId;
                        string savePath;
                        do
                        {
                            fileId = YZAttachmentHelper.GetNewFileID();
                            savePath = Attachment.FileIDToPath(fileId, YZAttachmentHelper.AttachmentRootPath);
                        } while (File.Exists(savePath));

                        Directory.CreateDirectory(savePath.Substring(0, savePath.LastIndexOf(@"\")));
                        file.SaveAs(savePath);

                        Attachment attachment = new Attachment();
                        attachment.FileID = fileId;
                        attachment.Name = fileName;
                        attachment.Ext = fileExt;
                        attachment.Size = fileSize;
                        attachment.LastUpdate = DateTime.Now;
                        attachment.OwnerAccount = User.Identity.Name;
                        attachments.Add(attachment);
                        using (IDbConnection cn = QueryManager.CurrentProvider.OpenConnection())
                        {
                            QueryManager.CurrentProvider.InsertAttachmentInfo(cn, attachment);
                        }
                    }
                }
                return Json(attachments);

            }
            catch (Exception e)
            {
                log.Error("error:", e);
            }
            return null;
           
        }

        /// <summary> 
        /// 获取附件详细信息 
        /// </summary> 
        /// <param name="attachId"></param> 
        /// <returns></returns> 
       [HttpPost]
        public IHttpActionResult GetAttachmentsInfo(PostAttachmentModel pach)
        {
            String[] fileIds=pach.fileIds;
            List<Attachment> attachments = new List<Attachment>();
            using (IDbConnection cn = QueryManager.CurrentProvider.OpenConnection())
            {
                var dr = QueryManager.CurrentProvider.GetAttachmentsInfo(cn, fileIds);
                while (dr.Read())//判断数据表中是否含有数据 
                {
                    var attach = new Attachment()
                    {
                        FileID = dr["FileID"].ToString(),
                        Name = dr["Name"].ToString(),
                        Ext = dr["Ext"].ToString(),
                        Size = long.Parse(dr["Size"].ToString()),
                        LastUpdate = (DateTime)dr["LastUpdate"],
                        OwnerAccount = dr["OwnerAccount"].ToString()

                    };
                    attachments.Add(attach);
                }
            }
            return Json(attachments);
        }

        /// <summary>
        /// 获取BPM用户
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetBPMParam()
        {
            PrepareBPMEnv();
           
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                MemberCollection positions = OrgSvr.GetUserPositions(cn, User.Identity.GetUserName());
                string UserFullName = positions[0].FullName;
                var bpmUser = BPM.Client.User.FromAccount(cn, User.Identity.GetUserName());

                return Json(new { Position = positions, BPMUser = bpmUser });
            }
            
        }
        /// <summary>
        /// 获取审批信息
        /// </summary>
        /// <param name="stepID"></param>
        /// <returns></returns>
        public IHttpActionResult GetProcessData(int stepID, int taskID)
        {
            PrepareBPMEnv();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                FlowDataSet formdataset = BPMProcess.GetFormData(cn, stepID);
                var tables=formdataset.Tables;
                
                Dictionary<string, DataTable> dt = new Dictionary<string, DataTable>();

                for(int i=0;i<tables.Count;i++)
                {
                    dt.Add(tables[i].TableName, tables[i].ToDataTable());
                }
                BPM.Client.ProcessInfo processInfo = BPMProcess.GetProcessInfo(cn, stepID);
                var steps = GetBPMStep(taskID);
                return Json(new { FormDataSet = dt, Process = processInfo ,Steps=steps});
                
            }
        }
        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public IHttpActionResult GetTaskData(int taskID)
        {
            PrepareBPMEnv();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                FlowDataSet formdataset= BPMProcess.GetFormDataForRead(cn, taskID);
                var tables = formdataset.Tables;
                Dictionary<string, DataTable> dt = new Dictionary<string, DataTable>();

                for (int i = 0; i < tables.Count; i++)
                {
                    dt.Add(tables[i].TableName, tables[i].ToDataTable());
                }
                var steps = GetBPMStep(taskID);
                return Json(new { FormDataSet = dt, Steps = steps });
            }
        }
        /// <summary>
        /// 获取审批步骤
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        private List<BPMProcStep> GetBPMStep(int taskID)
        {
            PrepareBPMEnv();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                BPMStepCollection steps = BPMTask.GetAllSteps(cn, taskID);
                var ret=steps.Where(e => e.IsHumanStep)
                    .Where(e => (!string.IsNullOrEmpty(e.OwnerAccount) || e.Share))
                    .ToList();
                return ret;
            }
        }
        /// <summary>
        /// 向BPM发起请求前设置环境
        /// </summary>
        private void PrepareBPMEnv()
        {
            var user_account = User.Identity.GetUserName();
            //user_account = "00000088";
            YZAuthHelper.SetAuthCookie(user_account);
            YZAuthHelper.ClearLogoutFlag();
        }
        /// <summary>
        /// 从Process.xml读取手机端支持的流程列表
        /// </summary>
        /// <returns></returns>
        private List<Process> GetMobileProcess()
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Process.xml");
            XElement xe = XElement.Load(path);
            IEnumerable<XElement> elements =xe.Elements("Process");
            List<Process> processList = new List<Process>();
            foreach (var ele in elements)
            {
                Process process = new Process();
                process.Name = ele.Element("Name").Value;
                process.Path = ele.Element("Path").Value;
                process.CanCreate = ele.Element("CanCreate").Value.ToLower().Equals("true");
                process.CreatePage = ele.Element("CreatePage").Value;
                process.ViewUndoPage = ele.Element("ViewUndoPage").Value;
                process.ViewAskPage = ele.Element("ViewAskPage").Value;
                process.ViewDonePage = ele.Element("ViewDonePage").Value;
                process.Css = ele.Element("Css").Value;
                process.DisplayName = ele.Element("DisplayName").Value;
                process.Icon = ele.Element("Icon").Value;
                process.Group = ele.Element("Group").Value;
                process.Description = ele.Element("Description").Value;
                processList.Add(process);
            }
            return processList;
        }
        /// <summary>
        /// 获取全部待办任务
        /// </summary>
        /// <returns></returns>
        private BPMTaskListCollection GetMyTaskList()
        {
            PrepareBPMEnv();
            BPMTaskListCollection tasks = new BPMTaskListCollection();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                var mobileProcess = GetMobileProcess();
                foreach (var p in mobileProcess)
                {
                    int rowcount = 0;
                    var list = cn.GetMyTaskList("ProcessName='" + p.Name + "'", null, 0, int.MaxValue, out rowcount);
                    tasks.Append(list);
                }
            }
            return tasks;
        }
        /// <summary>
        /// 获取全部我发起的
        /// </summary>
        /// <returns></returns>
        private BPMTaskCollection GetMyRequestList()
        {
            PrepareBPMEnv();
            BPMTaskCollection tasks = new BPMTaskCollection();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                var mobileProcess = GetMobileProcess();
                foreach (var p in mobileProcess)
                {
                    int rowcount = 0;
                    var list = cn.GetHistoryTasks( 
                        HistoryTaskType.MyRequest,p.Path,
                        "ProcessName='" + p.Name + "'", 
                        "", 
                        "", 
                        0, 
                        int.MaxValue,
                        out rowcount);
                    tasks.Append(list);
                }
            }
            return tasks;
        }
        /// <summary>
        /// 获取全部已办的
        /// </summary>
        /// <returns></returns>
        private BPMTaskCollection GetMyProcessedList()
        {
            PrepareBPMEnv();
            BPMTaskCollection tasks = new BPMTaskCollection();
            using (BPMConnection cn = new BPMConnection())
            {
                cn.WebOpen();
                var mobileProcess = GetMobileProcess();
                foreach (var p in mobileProcess)
                {
                    int rowcount = 0;
                    var list = cn.GetHistoryTasks(
                        HistoryTaskType.MyProcessed, p.Path,
                        "ProcessName='" + p.Name + "'",
                        "",
                        "",
                        0,
                        int.MaxValue,
                        out rowcount);
                    tasks.Append(list);
                }
            }
            return tasks;
        }
    }

    

   
    /// <summary>
    /// 流程Model,从Process.xml读入 
    /// </summary>
    public class Process
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public bool CanCreate { get; set; }
        public string CreatePage { get; set; }
        public string ViewUndoPage { get; set; }

        public string ViewAskPage { get; set; }

        public string ViewDonePage { get; set; }
        public string Css { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public string Group { get; set; }

        public string Description { get; set; }
    }
    public class TaskDto
    {
        public string Icon { get; set; }
        public string ViewUndoPage { get; set; }
  
        public string ViewAskPage { get; set; }
        public string ViewDonePage { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ReceiveAt { get; set; }
        public string Description { get; set; }
        public int TaskID { get; set; }
        public int StepID { get; set; }
        public string StepName { get; set; }
        public string ProcessName { get; set; }
       
        public TaskState TaskState { get; set; }
    }

    public class PostAccountInput
    {
        public string[] ids { get; set; }
    }
}
