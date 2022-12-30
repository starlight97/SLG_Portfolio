
// objectType
// 0 : Building, 1 : Ruck, 2 : Gathering
public class ObjectInfo
{
    public string sceneName;
    public int posX;
    public int posY;
    public int objectId;
    public int objectType;

    public ObjectInfo(string sceneName, int posX, int posY, int objectId, int objectType)
    {
        this.sceneName = sceneName;
        this.posX = posX;
        this.posY = posY;
        this.objectId = objectId;
        this.objectType = objectType;
    }

    public ObjectInfo()
    {

    }
}
