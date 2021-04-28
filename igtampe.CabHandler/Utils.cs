using Microsoft.Deployment.Compression;
using Microsoft.Deployment.Compression.Cab;
using System;
using System.IO;


namespace igtampe.CabHandler {

    /// <summary>Quick alias files </summary>
    public static class Utils{

        /// <summary>Indicates whether or not to show the progress bar</summary>
        public static bool Silent { get; set; } = true;

        /// <summary>Test</summary>
        public static void Main(string[] args) {

            Silent = false;

            if(args.Length != 2) {Help(); return;}
            try {
                if(args[0].EndsWith(".cab")) {
                    //assume extract
                    CabToFolder(args[0],args[1]);
                    return;
                } else if(args[1].EndsWith(".cab")) {
                    //assume compress
                    FolderToCab(args[0],args[1]);
                    
                    return;
                }
            } catch(Exception E) {
                Console.WriteLine("An error has occurred\n\n" + E.GetType() + ":" + E.Message + "\n\n" + E.StackTrace) ;
                return;
            }

            Help();
            return;
                    
        }

        public static void Help() {
            Console.WriteLine(
                "CabHandler Version 1.0\n" +
                "(C)2021 Igtampe, No rights reserved\n" +
                "\n" +
                "\n" +
                "Usage: [CAB] [DIR]\n" +
                "\n" +
                "CAB : Cab to extract\n" +
                "DIR : Directory to extract it to\n" +
                "\n" +
                "OR\n" +
                "\n" +
                "Usage: [DIR] [CAB]\n" +
                "\n" +
                "DIR : Directory to compress into a Cab\n" +
                "CAB : Cabinet to compress dir to\n" +
                "\n" +
                "WARNING: Cabinet will be overwritten!");
            Console.ReadLine();
        
        }

        /// <summary>Extracts all contents of a cab to a folder</summary>
        /// <param name="Cab"></param>
        /// <param name="Folder"></param>
        public static void CabToFolder(string Cab, string Folder) {
            CabInfo C = new CabInfo(Cab);
            C.Unpack(Folder,ProgHandler);
        }

        /// <summary>Creates a new cabinet file from the specified folder. WILL OVERWRITE ANY EXISTING CABINET FILE</summary>
        /// <param name="Folder"></param>
        /// <param name="Cab"></param>
        public static void FolderToCab(string Folder, string Cab) {
            if(File.Exists(Cab)) { File.Delete(Cab); }
            CabInfo C = new CabInfo(Cab);
            C.Pack(Folder,true,CompressionLevel.None,ProgHandler);
        }

        /// <summary>Shortcut to extracting a cab to a temp directory</summary>
        /// <param name="Cab"></param>
        /// <param name="TempDir"></param>
        public static void CabToTemp(string Cab, string TempDir) { CabToFolder(Cab,Path.Combine(Path.GetTempPath(),TempDir)); }

        /// <summary>Shortcut to creating a cab from a temp directory</summary>
        /// <param name="TempDir"></param>
        /// <param name="Cab"></param>
        public static void TempToCab(string TempDir, string Cab) { FolderToCab(Path.Combine(Path.GetTempPath(),TempDir),Cab); }

        private static void ProgHandler(object sender,ArchiveProgressEventArgs e) {
            //draw a small progress bar.
            Console.SetCursorPosition(0,Console.CursorTop);
            Console.Write("[          ] " + " Processing " + e.CurrentArchiveName + "... " + e.FileBytesProcessed + "/" + e.TotalFileBytes);

            //determine the progress:
            int progress =Convert.ToInt32((( e.FileBytesProcessed + 0.0) / e.TotalFileBytes)*10);

            Console.SetCursorPosition(1,Console.CursorTop);

            for(int i = 0; i < progress; i++) {Console.Write("#");}
        }
    }
}
