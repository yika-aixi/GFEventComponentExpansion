//创建者:Icarus
//手动滑稽,滑稽脸
//ヾ(•ω•`)o
//2018年09月03日-09:31
//Icarus.UnityGameFramework.Runtime

using System;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace Icarus.UnityGameFramework.Runtime
{
    public static class EventComponentExpansion
    {
        /// <summary>
        /// GameEventArgs 事件注册
        /// </summary>
        /// <param name="self"></param>
        /// <param name="handle"></param>
        /// <typeparam name="T"></typeparam>
        public static void GameEventSubscribe<T>(this EventComponent self,EventHandler<GameEventArgs> handle) where T : GameEventArgs, new()
        {
            self.GameEventSubscribe(typeof(T), handle);
        }
        
        /// <summary>
        /// GameEventArgs 事件注册
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameEventArgsType"></param>
        /// <param name="handle"></param>
        /// <exception cref="GameFrameworkException"></exception>
        public static void GameEventSubscribe(this EventComponent self,Type gameEventArgsType,EventHandler<GameEventArgs> handle)
        {
            if (!typeof(GameEventArgs).IsAssignableFrom(gameEventArgsType))
            {
                throw new GameFrameworkException($"gameEventArgsType 不是 {typeof(GameEventArgs)} 类型");
            }
            
            var args =  (GameEventArgs)ReferencePool.Acquire(gameEventArgsType);
            
            if (!self.Check(args.Id, handle))
            {
                self.Subscribe(args.Id, handle);
            }
            else
            {
                GameFrameworkLog.Error($"事件ID:{args.Id},已存在相同的处理函数,无法继续注册.");
            }

            ReferencePool.Release(args);
        }
        
        /// <summary>
        /// GameEventArgs 事件释放
        /// </summary>
        /// <param name="self"></param>
        /// <param name="handle"></param>
        /// <typeparam name="T"></typeparam>
        public static void GameEventUnsubscribe<T>(this EventComponent self,EventHandler<GameEventArgs> handle) where T : GameEventArgs, new()
        {
            self.GameEventUnsubscribe(typeof(T), handle);
        }
        
        /// <summary>
        /// GameEventArgs 事件释放
        /// </summary>
        /// <param name="self"></param>
        /// <param name="gameEventArgsType"></param>
        /// <param name="handle"></param>
        /// <exception cref="GameFrameworkException"></exception>
        public static void GameEventUnsubscribe(this EventComponent self,Type gameEventArgsType,EventHandler<GameEventArgs> handle)
        {
            if (!typeof(GameEventArgs).IsAssignableFrom(gameEventArgsType))
            {
                throw new GameFrameworkException($"gameEventArgsType 不是 {typeof(GameEventArgs)} 类型");
            }
            
            var args =  (GameEventArgs)ReferencePool.Acquire(gameEventArgsType);

           
            if (self.Check(args.Id,handle))
            {
                self.Unsubscribe(args.Id,handle);
            }
            else
            {
                GameFrameworkLog.Warning($"事件ID:{args.Id},已经被释放了,无法再次释放.");
            }
            
            ReferencePool.Release(args);
        }
    }
}