using Injector40K;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace _40KPawnRenderer
{
    public class ModInitializer : ITab
    {
        protected GameObject modInitializerControllerObject;


        public ModInitializer()
        {
            LongEventHandler.QueueLongEvent(delegate
            {
                Log.Message("Initialized 40k Corruption Mod");
                this.modInitializerControllerObject = new GameObject("Corruptoid");
                this.modInitializerControllerObject.AddComponent<ModInitializerBehaviour>();
                this.modInitializerControllerObject.AddComponent<DoOnMainThread>();
                UnityEngine.Object.DontDestroyOnLoad(this.modInitializerControllerObject);
            }, "queueInject", false, null);
        }

        protected override void FillTab()
        { }

        public override void TabTick()
        {
            // Log.Message("TryingTOStart");
        }
    }

    public class DoOnMainThread : MonoBehaviour
    {

        public static readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();

        public void Update()
        {
            while (ExecuteOnMainThread.Count > 0)
            {
                ExecuteOnMainThread.Dequeue().Invoke();
            }
        }
    }



    class ModInitializerBehaviour : MonoBehaviour
    {

        public void FixedUpdate()
        {
        }

        public void OnLevelWasLoaded()
        {
        }

        public void Start()
        {
            Log.Message("Initiated PawnRenderer Detours.");

            MethodInfo method1a = typeof(Verse.PawnRenderer).GetMethod("RenderPawnInternal", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Vector3), typeof(Quaternion), typeof(Boolean), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(Boolean) }, null);
            MethodInfo method1b = typeof(PawnRendererModded).GetMethod("_RenderPawnInternal", new Type[] { typeof(PawnRenderer), typeof(Vector3), typeof(Quaternion), typeof(Boolean), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(Boolean) }, null);

            try
            {
                Detours.TryDetourFromTo(method1a, method1b);


                Log.Message("PawnRenderer methods detoured!");
            }
            catch (Exception)
            {
                Log.Error("Could not detour PawnRenderer");
                throw;
            }

        }
    }
}
