using System;
using System.Collections.Generic;
using Godot;
using GodotTemplate.Scenes.World.ClientScenes;
using GodotTemplate.Scenes.World.Data;
using GodotTemplate.Scenes.World.Data.MapPoint;
using GodotTemplate.Scenes.World.SyncedScenes;
using GodotTemplate.Scenes.World.Tree;
using GodotTemplate.Scenes.World.Tree.Entity.Building;
using KludgeBox.DI.Requests.ChildInjection;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;
using PersistenceNodesFactoryService = GodotTemplate.Scenes.World.PersistenceFactory.PersistenceNodesFactoryService;
using WorldStartStopService = GodotTemplate.Scenes.World.StartStop.WorldStartStopService;

namespace GodotTemplate.Scenes.World;

/// <summary>
/// Является хранилищем сервисов. Каждый сервис может ссылаться на другие сервисы.
/// Каждый сервис является точкой взаимодействия с системой, и при вызове методов должен гарантировать,
/// что он внесёт изменения и в другие сервисы, чтобы сохранить целостность состояния системы.
/// </summary>
public partial class World : Node2D, IServiceProvider
{
    
    [Child] public WorldTree Tree { get; private set; }
    [Child] public WorldPersistenceData Data { get; private set; }
    [Child] public PersistenceNodesFactoryService Factory { get; private set; }
    [Child] public WorldTemporaryDataService TemporaryData { get; private set; }
    [Child] public WorldStartStopService StartStop { get; private set; }
    [Child] public WorldMultiplayerSpawnerService MultiplayerSpawner { get; private set; }
    [Child] public SyncedPackedScenes SyncedPackedScenes { get; private set; }
    [Child] public ClientPackedScenes ClientPackedScenes { get; private set; }
    
    public readonly WorldEvents Events = new();
    
    private readonly Dictionary<Type, object> _services = new();
    [Logger] private ILogger _log;

    public override void _EnterTree() 
    {
        Di.Process(this);
        
        AddService(Tree);
        AddService(Data);
        AddService(Factory);
        AddService(TemporaryData);
        AddService(StartStop);
        AddService(MultiplayerSpawner);
        AddService(SyncedPackedScenes);
        AddService(ClientPackedScenes);
    }

    public object GetService(Type serviceType)
    {
        return _services.GetValueOrDefault(serviceType, null);
    }
    
    private void AddService(object service)
    {
        if (!_services.ContainsKey(service.GetType()))
        {
            _log.Warning("Service {type} already exists", service.GetType());
        }
        else
        {
            _services.Add(service.GetType(), service);
        }
    }

    //TODO Test methods. Remove after tests.
    public void Test1() => RpcId(ServerId, MethodName.Test1Rpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void Test1Rpc()
    {
        _log.Warning("Test 1 RPC called");
        
        Tree.MapSurface.AddChildWithUniqueName(Factory.Create<MapPoint, MapPointData>(data =>
        {
            data.PositionX = Random.Shared.Next(0, 600);
            data.PositionY = Random.Shared.Next(0, 600);
        }), "MapPoint");
    }
    
    public void Test2() => RpcId(ServerId, MethodName.Test2Rpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void Test2Rpc()
    {
        _log.Warning("Test 2 RPC called");
    }
    
    public void Test3() => RpcId(ServerId, MethodName.Test3Rpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void Test3Rpc()
    {
        _log.Warning("Test 3 RPC called");
    }
    
    //TODO Переделать на нормальный метод запроса сохранения с клиента на сервер, с проверкой прав
    public void TestSave(string saveFileName) => RpcId(ServerId, MethodName.TestSaveRpc, saveFileName);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void TestSaveRpc(string saveFileName)
    {
        _log.Warning("TestSave RPC called");
        Data.SaveLoad.Save(saveFileName);
    }
}