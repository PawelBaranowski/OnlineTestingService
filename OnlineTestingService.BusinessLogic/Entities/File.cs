using System;
using System.IO;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// This class provides an abstract represantation of a file stored in database.
    /// </summary>
    public class File : EntityWithId<File>, CanDelete
    {
        private string name;
        private byte[] content;

        /// <summary>
        /// Gets name of the file this object represents.
        /// </summary>
        public virtual string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }

        public virtual string ContentType { get; private set; }

        /// <summary>
        /// Gets the content of the file this object represents.
        /// </summary>
        public virtual byte[] Content
        {
            get { return this.content; }
            private set { this.content = value; }
        }

        /// <summary>
        /// Creates file to store in database.
        /// </summary>
        /// <param name="file">HttpPostedFile is an object created by FileUpload 
        /// (web control) when uploading a file.</param>
        internal File(System.Web.HttpPostedFileBase file)
        {            
            this.name = file.FileName;
            this.content = new byte[file.ContentLength];
            this.ContentType = file.ContentType;
            file.InputStream.Read(this.content, 0, file.ContentLength);
        }

        /// <summary>
        /// 
        /// </summary>
        public File()
        {
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((File)obj).Content.Equals(this.Content);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Content.GetHashCode();
        }
    }
}
