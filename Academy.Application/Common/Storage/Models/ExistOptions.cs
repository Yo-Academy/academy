namespace Academy.Application.Common.Storage.Models
{
    public class ExistOptions : BaseOptions
    {
        public static ExistOptions FromBaseOptions(BaseOptions options)
        {
            return new ExistOptions { FileName = options.FileName, Directory = options.Directory };
        }
    }
}
