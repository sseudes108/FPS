using System;
using System.IO;
using UnityEngine;

public class FileDataHandler{
    private string _dataDirPath;
    private string _dataFileName;

    public FileDataHandler(string dataDirPath, string dataFileName){
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
    }

    public GameData Load(){
        string fullPath  = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath)){
            try{
                string dataToLoad;
                using(FileStream stream = new FileStream(fullPath, FileMode.Open)){
                    using(StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }catch (Exception e){
                Debug.LogError($"Error when try read the saved file {fullPath} + '\n' {e}");
            }
        }
        return loadedData;
    }

    public void Save(GameData data){
        string fullPath  = Path.Combine(_dataDirPath, _dataFileName);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            string dataToStore = JsonUtility.ToJson(data, true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create)){
                using(StreamWriter writer = new StreamWriter(stream)){
                    writer.WriteLine(dataToStore);
                }
            }

        }catch (Exception e){
            Debug.LogError($"Error when try to save data to file {fullPath} + '\n' {e}");
        }
    }
}