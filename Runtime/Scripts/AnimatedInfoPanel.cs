using System.Collections.Generic;
using UnityEngine;

public class AnimatedInfoPanel : MonoBehaviour
{
    private Dictionary<string, InfoGroup> groups = new Dictionary<string, InfoGroup>();

    public class InfoGroup
    {
        public string InfoName;
        public List<AnimatedInfo> infos;

        public InfoGroup()
        {
            infos = new List<AnimatedInfo>();
        }
    }
    public AnimatedInfo InstantiateInfo(AnimatedInfo source)
    {
        return InstantiateInfo(source, int.MaxValue);
    }
    public AnimatedInfo InstantiateInfo(AnimatedInfo source, int maxCount)
    {
        var info = Instantiate(source, transform);
        info.Play(this);

        if(!groups.TryGetValue(info.InfoName, out InfoGroup group))
        {
            group = new InfoGroup();
            groups.Add(info.InfoName, group);
        }

        if(group.infos.Count >= maxCount)
        {
            Destroy(group.infos[0].gameObject);
            group.infos.RemoveAt(0);
        }

        group.infos.Add(info);

        return info;
    }
    internal void EndInfo(AnimatedInfo info)
    {
        groups[info.InfoName].infos.Remove(info);
        Destroy(info.gameObject);
    }
}