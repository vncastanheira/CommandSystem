using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CSoft
{
    public static class CommandSystem
    {
        private static readonly Dictionary<Type, List<Delegate>> _oneWayEvents = new();
        private static readonly Dictionary<Type, List<Delegate>> _twoWayEvents = new();

        #region Listen
        public static void ListenTo<T>(Action<T> callback) where T : struct
        {
            if (_oneWayEvents.TryGetValue(typeof(T), out var events))
            {
                events.Add(callback);
            }
            else
            {
                _oneWayEvents.Add(typeof(T), new List<Delegate> { callback });
            }
        }

        public static async Task ListenToAsync<T>(Action<T> callback) where T : struct
        {
            await Task.Run(() =>
            {
                if (_oneWayEvents.TryGetValue(typeof(T), out var events))
                {
                    events.Add(callback);
                }
                else
                {
                    _oneWayEvents.Add(typeof(T), new List<Delegate> { callback });
                }
            });
        }

        public static void ListenTo<T1, T2>(Func<T1, T2> callback) where T1 : struct
        {
            if (_twoWayEvents.TryGetValue(typeof(Func<T1, T2>), out var events))
            {
                events.Add(callback);
            }
            else
            {
                _twoWayEvents.Add(typeof(Func<T1, T2>), new List<Delegate> { callback });
            }
        }

        public static async Task ListenToAsync<T1, T2>(Func<T1, T2> callback) where T1 : struct
        {
            await Task.Run(() =>
            {
                if (_twoWayEvents.TryGetValue(typeof(Func<T1, T2>), out var events))
                {
                    events.Add(callback);
                }
                else
                {
                    _twoWayEvents.Add(typeof(Func<T1, T2>), new List<Delegate> { callback });
                }
            });
        }

        #endregion

        #region Trigger
        public static void Trigger<T>(T e) where T : struct
        {
            if (_oneWayEvents.TryGetValue(typeof(T), out var events))
            {
                foreach (var ev in events)
                {
                    var callback = ev as Action<T>;
                    callback(e);
                }
            }
        }

        public static async Task TriggerAsync<T>(T e) where T : struct
        {
            await Task.Run(() =>
            {
                if (_oneWayEvents.TryGetValue(typeof(T), out var events))
                {
                    foreach (var ev in events)
                    {
                        var callback = ev as Action<T>;
                        callback(e);
                    }
                }
            });
        }

        public static List<T2> Trigger<T1, T2>(T1 e) where T1 : struct
        {
            var returningValues = new List<T2>();
            if (_twoWayEvents.TryGetValue(typeof(Func<T1, T2>), out var events))
            {
                foreach (var ev in events)
                {
                    var callback = ev as Func<T1, T2>;
                    returningValues.Add(callback(e));
                }
            }
            return returningValues;
        }

        public static async Task<List<T2>> TriggerAsync<T1, T2>(T1 e) where T1 : struct
        {
            return await Task.Run(() =>
            {
                var returningValues = new List<T2>();
                if (_twoWayEvents.TryGetValue(typeof(Func<T1, T2>), out var events))
                {
                    foreach (var ev in events)
                    {
                        var callback = ev as Func<T1, T2>;
                        returningValues.Add(callback(e));
                    }
                }
                return returningValues;
            });
        }
        #endregion

        #region Unlisten
        public static void UnlistenTo<T>(Action<T> action) where T : struct
        {
            if (_oneWayEvents.TryGetValue(typeof(T), out var events))
            {
                events.Remove(action);
            }
            else
            {
                throw new UnityException("Could not find specified action in list of events.");
            }
        }

        public static async void UnlistenToAsync<T>(Action<T> action) where T : struct
        {
            await Task.Run(() =>
            {
                if (_oneWayEvents.TryGetValue(typeof(T), out var events))
                {
                    events.Remove(action);
                }
                else
                {
                    throw new UnityException("Could not find specified action in list of events.");
                }
            });
        }

        public static void UnlistenTo<T1, T2>(Func<T1, T2> action) where T1 : struct
        {
            if (_twoWayEvents.TryGetValue(typeof(Func<T1, T2>), out var events))
            {
                events.Remove(action);
            }
            else
            {
                throw new UnityException("Could not find specified action in list of events.");
            }
        }

        public static async void UnlistenToAsync<T1, T2>(Func<T1, T2> action) where T1 : struct
        {
            await Task.Run(() =>
            {
                if (_twoWayEvents.TryGetValue(typeof(Func<T1, T2>), out var events))
                {
                    events.Remove(action);
                }
                else
                {
                    throw new UnityException("Could not find specified action in list of events.");
                }
            });
        }
        #endregion
    }
}
