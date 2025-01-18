// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapList
{
    public delegate void MapListHandler(object sender);

    private List<int> userAllowedPassword = new List<int>();

    private ArrayList mapList = new ArrayList();

    private ArrayList allowedMapList;

    private static MapList instance;

    private ArrayList mapListByMode = new ArrayList();

    private MapMode.MODE mapListModeFilter;

    public static MapList Instance
    {
        get
        {
            if (MapList.instance == null)
            {
                MapList.instance = new MapList();
            }
            return MapList.instance;
        }
    }

    public Map this[int index]
    {
        get
        {
            return (Map)MapList.Instance.mapList[index];
        }
    }

    public Map this[string systemName]
    {
        get
        {
            foreach (Map map in MapList.Instance.mapList)
            {
                if (map.SystemName.Equals(systemName))
                {
                    return map;
                }
            }
            return null;
        }
    }

    public static event MapListHandler OnLoad;

    public static event MapListHandler OnTry;

    public static event MapListHandler OnBuy;

    public static event MapListHandler OnError;

    protected MapList()
    {
    }

    public static ArrayList GetMaps()
    {
        return MapList.Instance.mapList;
    }

    public static ArrayList GetAllowedMap()
    {
        if (MapList.Instance.allowedMapList == null)
        {
            MapList.Instance.allowedMapList = new ArrayList();
            foreach (Map map in MapList.Instance.mapList)
            {
                if (map.Buyed || !map.Tryed)
                {
                    MapList.Instance.allowedMapList.Add(map);
                }
            }
        }
        return MapList.Instance.allowedMapList;
    }

    public static ArrayList GetMaps(MapMode.MODE mode)
    {
        if (mode == MapMode.MODE.NONE)
        {
            return MapList.GetMaps();
        }
        if (mode == MapList.Instance.mapListModeFilter)
        {
            return MapList.Instance.mapListByMode;
        }
        MapList.Instance.mapListByMode.Clear();
        MapList.Instance.mapListModeFilter = mode;
        foreach (Map map in MapList.Instance.mapList)
        {
            if ((map.Modes & (int)mode) == (int)mode)
            {
                MapList.Instance.mapListByMode.Add(map);
            }
        }
        return MapList.Instance.mapListByMode;
    }

    public static string GetMapName(string sceneName)
    {
        Map map = MapList.GetMap(sceneName);
        if (map != null)
        {
            return map.Name;
        }
        return string.Empty;
    }

    public static Map GetMap(string systemName)
    {
        foreach (Map map in MapList.Instance.mapList)
        {
            if (map.SystemName.Equals(systemName))
            {
                return map;
            }
        }
        return null;
    }

    public void Init()
    {
        Ajax.Request(WebUrls.MAP_URL, new AjaxRequest.AjaxHandler(MapList.Instance.OnLoadMaps));
    }

    private void OnLoadMaps(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSON.Parse(Ajax.DecodeUtf(result.ToString()));
        if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
        {
            if (jSONNode["s"] != (object)null)
            {
                ServerConf.Instance.InitServerList(jSONNode["s"]);
            }
            ServerConf.Instance.InitGmServerList(jSONNode["gm"]);
            this.mapList.Clear();
            Dictionary<int, MapState> dictionary = new Dictionary<int, MapState>();
            JSONNode jSONNode2 = jSONNode["b"];
            if (jSONNode2 != (object)null)
            {
                foreach (JSONNode child in jSONNode2.Childs)
                {
                    string value = child["n"].Value;
                    string[] players = child["p"].Value.Split(',');
                    int asInt = child["i"].AsInt;
                    int asInt2 = child["m"].AsInt;
                    int asInt3 = child["dp"].AsInt;
                    ShopCost shopCost = null;
                    if (child["sc"] != (object)null)
                    {
                        shopCost = new ShopCost(child["sc"]);
                    }
                    Map map = new Map(asInt, value, "map_" + value + "_name", "map_" + value + "_desc", asInt2, players, asInt3, shopCost);
                    if (dictionary.ContainsKey(map.MapID))
                    {
                        if (dictionary[map.MapID] == MapState.Try)
                        {
                            map.Tryed = true;
                        }
                        else if (dictionary[map.MapID] == MapState.Buyed)
                        {
                            map.Buyed = true;
                        }
                    }
                    if (shopCost == null)
                    {
                        map.Buyed = true;
                    }
                    this.mapList.Add(map);
                }
            }
            if (MapList.OnLoad != null)
            {
                MapList.OnLoad(this);
            }
            return;
        }
        if (MapList.OnError != null)
        {
            MapList.OnError(result);
            return;
        }
        throw new Exception("[MapList] OnLoadMaps not init: " + result);
    }

    public void TryMap(Map map)
    {
        map.Tryed = true;
    }

    public void BuyMap(Map map)
    {
        AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.MAP_BUY_URL + "&i=" + map.MapID, map);
        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(MapList.Instance.OnBuyMap);
        Ajax.Request(ajaxRequest);
    }

    private void OnBuyMap(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(result.ToString());
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            (request.Tag as Map).Buyed = true;
            LocalUser.SubShopCost((request.Tag as Map).ShopCost);
            MapList.Instance.allowedMapList = null;
            if (MapList.OnBuy != null)
            {
                MapList.OnBuy(request.Tag);
            }
            return;
        }
        if (MapList.OnError != null)
        {
            MapList.OnError(result);
            return;
        }
        throw new Exception("[MapList] OnBuyMap is not valid: " + result);
    }

    public void UnloadResource()
    {
        foreach (Map map in this.mapList)
        {
            map.UnloadIco();
        }
    }
}


