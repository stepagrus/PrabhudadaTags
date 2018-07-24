using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tagger.Model
{
  class PrabhupadaFileService
  {
    private const string Mp3Extension = ".mp3";
    private string _path;

    public PrabhupadaFileService(string pathToFolder)
    {
      _path = pathToFolder;
    }

    public void StartGettingFilesDescriptors()
    {

      var files = GetMp3FilesRecursively(_path).ToList();

      for (int i = 0; i < files.Count; i++)
      {
        var filePath = files[i];

        FileTemplate item = new FileTemplate(filePath);
        item.Mp3Length = GetMp3Length(filePath);

        NewFileParsed?.Invoke(this, item);
        ProgressUpdated?.Invoke(this, GetProgress(i + 1, files.Count));
      }
    }

    public static void RenameFile(FileTemplate template)
    {
      var sourcePath = template.SourcePath;
      var sourceDir = Path.GetDirectoryName(sourcePath);
      var newFileName = template.Filename + ".mp3";

      var newPath = Path.Combine(Path.GetDirectoryName(sourcePath), newFileName);
      File.Move(sourcePath, newPath);
      template.SourcePath = newPath;
    }

    public event EventHandler<FileTemplate> NewFileParsed;
    public event EventHandler<string> ProgressUpdated;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>List of file paths </returns>
    private IEnumerable<string> GetMp3FilesRecursively(string pathToFolder)
    {
      var files = Directory.EnumerateFiles(pathToFolder).Where(name => IsMp3File(name));
      var folders = Directory.EnumerateDirectories(pathToFolder);
      foreach (var folder in folders)
      {
        files = files.Union(GetMp3FilesRecursively(folder));
      }
      return files;
    }

    private bool IsMp3File(string filePath)
    {
      return Path.GetExtension(filePath).ToLower() == Mp3Extension;
    }

    private TimeSpan GetMp3Length(string filePath)
    {
      using (var info = TagLib.File.Create(filePath))
      {
        return info.Properties.Duration.Subtract(TimeSpan.FromMilliseconds(info.Properties.Duration.Milliseconds));
      }
    }

    private string GetProgress(int current, int total)
    {
      string cur = $"{current.ToString().PadLeft(4, '0')}";
      string tot = $"{total.ToString().PadLeft(4, '0')}";
      return cur + "/" + tot;
    }

  }
}
