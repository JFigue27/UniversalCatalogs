using Reusable.Attachments;
using Reusable.Utils;
using ServiceStack;
using ServiceStack.Configuration;
using System;
using System.IO;
using System.Net;

namespace Reusable.Rest.Implementations.SS
{
    [Restrict(LocalhostOnly = true)]
    public class AttachmentService : Service
    {
        public static IAppSettings AppSettings { get; set; }

        public object Get(DownloadAttachment request)
        {
            bool useAttachmentsRelativePath = false;
            string sUseAttachmentsRelativePath = AppSettings.Get<string>("UseAttachmentsRelativePath");
            if (!string.IsNullOrWhiteSpace(sUseAttachmentsRelativePath) && bool.TryParse(sUseAttachmentsRelativePath, out bool bUseAttachmentsRelativePath))
            {
                useAttachmentsRelativePath = bUseAttachmentsRelativePath;
            }

            string strDirectory = request.Directory;
            string strFileName = request.FileName;
            string appSettingsFolder = request.AttachmentKind;
            string baseAttachmentsPath = AppSettings.Get<string>(appSettingsFolder);

            string filePath;
            if (useAttachmentsRelativePath)
            {
                filePath = "~/" + baseAttachmentsPath + strDirectory + "/" + strFileName.MapHostAbsolutePath();
            }
            else
            {
                filePath = baseAttachmentsPath + strDirectory + "\\" + strFileName;
            }

            var file = new FileInfo(filePath);

            Response.StatusCode = 200;
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.AddHeader("Content-Disposition", "attachment; filename=\"{0}\"".Fmt(file.Name));

            return new HttpResult(file, true);
        }

        public object Get(DeleteAttachment request)
        {
            bool useAttachmentsRelativePath = false;
            string sUseAttachmentsRelativePath = AppSettings.Get<string>("UseAttachmentsRelativePath");
            if (!string.IsNullOrWhiteSpace(sUseAttachmentsRelativePath) && bool.TryParse(sUseAttachmentsRelativePath, out bool bUseAttachmentsRelativePath))
            {
                useAttachmentsRelativePath = bUseAttachmentsRelativePath;
            }

            string strDirectory = request.Directory;
            string strFileName = request.FileName;
            string appSettingsFolder = request.AttachmentKind;
            string baseAttachmentsPath = AppSettings.Get<string>(appSettingsFolder);

            string filePath;
            if (useAttachmentsRelativePath)
            {
                filePath = "~/" + baseAttachmentsPath + strDirectory + "/" + strFileName.MapHostAbsolutePath();
            }
            else
            {
                filePath = baseAttachmentsPath + strDirectory + "\\" + strFileName;
            }

            FileInfo file = new FileInfo(filePath);
            file.Delete();

            return new HttpResult();
        }

        public object Post(PostAttachment request)
        {
            var response = new CommonResponse();

            string attachmentKind = request.AttachmentKind;
            if (attachmentKind == null || attachmentKind == "")
            {
                response.ErrorThrown = true;
                response.ResponseDescription = "Attachment Kind was not specified.";
                return response;
            }

            if (Request.Files.Length == 0)
                throw new HttpError(HttpStatusCode.BadRequest, "NoFile");

            var postedFile = Request.Files[0];

            string fileName = postedFile.FileName;

            string baseAttachmentsPath = AppSettings.Get<string>(attachmentKind);

            bool useAttachmentsRelativePath = false;
            string sUseAttachmentsRelativePath = AppSettings.Get<string>("UseAttachmentsRelativePath");
            if (!string.IsNullOrWhiteSpace(sUseAttachmentsRelativePath) && bool.TryParse(sUseAttachmentsRelativePath, out bool bUseAttachmentsRelativePath))
            {
                useAttachmentsRelativePath = bUseAttachmentsRelativePath;
            }

            string currentPathAttachments;
            string folderName = request.TargetFolder;
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                if (useAttachmentsRelativePath)
                {
                    currentPathAttachments = "~/" + baseAttachmentsPath + folderName + @"/".MapHostAbsolutePath();
                }
                else
                {
                    currentPathAttachments = baseAttachmentsPath + folderName + @"\";
                }

                if (!Directory.Exists(currentPathAttachments))
                {
                    Directory.CreateDirectory(currentPathAttachments);
                }
            }
            else
            {
                string folderPrefix = request.FolderPrefix;
                if (string.IsNullOrWhiteSpace(folderPrefix) || folderPrefix == "undefined" || folderPrefix == "null")
                {
                    folderPrefix = "";
                }
                do
                {
                    DateTime date = DateTime.Now;
                    folderName = folderPrefix + date.ToString("yy") + date.Month.ToString("d2") +
                                    date.Day.ToString("d2") + "_" + MD5HashGenerator.GenerateKey(date);

                    if (useAttachmentsRelativePath)
                    {
                        currentPathAttachments = "~/" + baseAttachmentsPath + folderName.MapHostAbsolutePath();
                    }
                    else
                    {
                        currentPathAttachments = baseAttachmentsPath + folderName;
                    }
                } while (Directory.Exists(currentPathAttachments));
                Directory.CreateDirectory(currentPathAttachments);
                if (useAttachmentsRelativePath)
                {
                    currentPathAttachments += @"/";
                }
                else
                {
                    currentPathAttachments += @"\";
                }
            }

            if (postedFile.ContentLength > 0)
            {
                postedFile.SaveTo(currentPathAttachments + Path.GetFileName(postedFile.FileName));
            }

            var attachmentsResult = AttachmentsIO.getAttachmentsFromFolder(folderName, attachmentKind);

            response.ErrorThrown = false;
            response.ResponseDescription = folderName;
            response.Result = attachmentsResult;
            return response;
        }

        public object Post(PostAvatar request)
        {
            var response = new CommonResponse();

            if (Request.Files.Length == 0)
                throw new HttpError(HttpStatusCode.BadRequest, "NoFile");

            var postedFile = Request.Files[0];

            string fileName = postedFile.FileName;

            string baseAttachmentsPath = AppSettings.Get<string>("Avatar");

            bool useAttachmentsRelativePath = false;
            string sUseAttachmentsRelativePath = AppSettings.Get<string>("UseAttachmentsRelativePath");
            if (!string.IsNullOrWhiteSpace(sUseAttachmentsRelativePath) && bool.TryParse(sUseAttachmentsRelativePath, out bool bUseAttachmentsRelativePath))
            {
                useAttachmentsRelativePath = bUseAttachmentsRelativePath;
            }

            string currentPathAttachments;
            string folderName = request.TargetFolder;
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                if (useAttachmentsRelativePath)
                {
                    currentPathAttachments = "~/" + baseAttachmentsPath + folderName + @"/".MapHostAbsolutePath();
                }
                else
                {
                    currentPathAttachments = baseAttachmentsPath + folderName + @"\";
                }

                if (!Directory.Exists(currentPathAttachments))
                {
                    Directory.CreateDirectory(currentPathAttachments);
                }
                else
                {
                    AttachmentsIO.ClearDirectory(currentPathAttachments);
                }
            }
            else
            {
                do
                {
                    DateTime date = DateTime.Now;
                    folderName = date.ToString("yy") + date.Month.ToString("d2") +
                                    date.Day.ToString("d2") + "_" + MD5HashGenerator.GenerateKey(date);

                    if (useAttachmentsRelativePath)
                    {
                        currentPathAttachments = "~/" + baseAttachmentsPath + folderName.MapHostAbsolutePath();
                    }
                    else
                    {
                        currentPathAttachments = baseAttachmentsPath + folderName;
                    }
                } while (Directory.Exists(currentPathAttachments));
                Directory.CreateDirectory(currentPathAttachments);
                if (useAttachmentsRelativePath)
                {
                    currentPathAttachments += @"/";
                }
                else
                {
                    currentPathAttachments += @"\";
                }
            }

            if (postedFile.ContentLength > 0)
            {
                postedFile.SaveTo(currentPathAttachments + Path.GetFileName(postedFile.FileName));
            }

            var attachmentsResult = AttachmentsIO.getAvatarsFromFolder(folderName, "Avatar");

            response.ErrorThrown = false;
            response.ResponseDescription = folderName;
            response.Result = attachmentsResult;
            return response;
        }
    }

    [Route("/Attachment/download", "GET")]
    public class DownloadAttachment : IReturn<byte[]>
    {
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string AttachmentKind { get; set; }
    }

    [Route("/Attachment/delete", "GET")]
    public class DeleteAttachment
    {
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string AttachmentKind { get; set; }
    }

    [Route("/Attachment/upload", "POST")]
    public class PostAttachment
    {
        public string AttachmentKind { get; set; }
        public string FileName { get; set; }
        public string UploadedByUserName { get; set; }
        public string TargetFolder { get; set; }
        public string FolderPrefix { get; set; }
    }

    [Route("/Avatar/upload", "POST")]
    public class PostAvatar
    {
        public string TargetFolder { get; set; }
    }
}
