#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class HonoScriptsBattleCoreDamagePipLineWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Hono.Scripts.Battle.Core.DamagePipLine);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 8, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAttackerFinalAttr", _m_GetAttackerFinalAttr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSkillModifierAttr", _m_GetSkillModifierAttr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetDamageResult", _m_SetDamageResult);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDamageResult", _m_GetDamageResult);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "DamageSourceType", _g_get_DamageSourceType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SourceAbilityId", _g_get_SourceAbilityId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HitUnitCount", _g_get_HitUnitCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FormulaName", _g_get_FormulaName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DamageRatio", _g_get_DamageRatio);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DamageType", _g_get_DamageType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AddiTypes", _g_get_AddiTypes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MultiTypes", _g_get_MultiTypes);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Hono.Scripts.Battle.Core.DamagePipLine();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Hono.Scripts.Battle.Core.DamagePipLine constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Hono.Scripts.Battle.Core.HitInfo _hitInfo;translator.Get(L, 2, out _hitInfo);
                    
                    gen_to_be_invoked.Init( _hitInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackerFinalAttr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Hono.Scripts.Battle.Core.EAttrType _attrType;translator.Get(L, 2, out _attrType);
                    
                        var gen_ret = gen_to_be_invoked.GetAttackerFinalAttr( _attrType );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSkillModifierAttr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Hono.Scripts.Battle.Core.EAttrType _attrType;translator.Get(L, 2, out _attrType);
                    
                        var gen_ret = gen_to_be_invoked.GetSkillModifierAttr( _attrType );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetDamageResult(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _damageValue = LuaAPI.xlua_tointeger(L, 2);
                    bool _isCitlt = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetDamageResult( _damageValue, _isCitlt );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Hono.Scripts.Battle.Core.DamageResult>(L, 2)) 
                {
                    Hono.Scripts.Battle.Core.DamageResult _result;translator.Get(L, 2, out _result);
                    
                    gen_to_be_invoked.SetDamageResult( _result );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Hono.Scripts.Battle.Core.DamagePipLine.SetDamageResult!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDamageResult(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetDamageResult(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DamageSourceType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                translator.PushHonoScriptsBattleEDamageSourceType(L, gen_to_be_invoked.DamageSourceType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SourceAbilityId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SourceAbilityId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HitUnitCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.HitUnitCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FormulaName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.FormulaName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DamageRatio(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DamageRatio);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DamageType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DamageType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AddiTypes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AddiTypes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MultiTypes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Hono.Scripts.Battle.Core.DamagePipLine gen_to_be_invoked = (Hono.Scripts.Battle.Core.DamagePipLine)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MultiTypes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
