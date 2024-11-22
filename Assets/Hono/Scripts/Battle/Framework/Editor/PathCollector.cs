using Hono.Scripts.Battle.Define;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Editor {

	public static class PathCollectorTools {
		[MenuItem("Tools/收集路径")]
		public static void Do() {
			PathCollector pathCollector = new PathCollector();
			pathCollector.CollectPath();
		}
	}
	
	public class PathCollector {
		private Paths _paths;
		
		public PathCollector() {
			_paths = AssetDatabase.LoadAssetAtPath<Paths>("Assets/BattleData/Paths.asset");
		}

		public void CollectPath() {
			_paths.paths.Clear();
			
			//csv
			// 获取文件夹中所有 .csv 文件的路径
			var csvPaths = new List<string>();
			string[] csvFiles = Directory.GetFiles(BattleConstValue.CSVRoot, "*.csv");
			foreach (string fullPath in csvFiles)
			{
				string fileName = Path.GetFileName(fullPath);
				string assetPath = BattleConstValue.CSVRoot + "/" + fileName;
				csvPaths.Add(assetPath);
			}
			_paths.paths.Add(EPathType.CSV,csvPaths);
			
			//ability
			if (TryGetAllAssetPaths(BattleConstValue.AbilityRoot, out var abilityPaths))
			{
				_paths.paths.Add(EPathType.Ability,abilityPaths);
			}
			else {
				Debug.LogError("Ability收集失败");
			}
			
			if (TryGetAllAssetPaths(BattleConstValue.SkillFolder, out var skillPaths))
			{
				_paths.paths.Add(EPathType.Skill,skillPaths);
			}
			else {
				Debug.LogError("Skill收集失败");
			}
			
			if (TryGetAllAssetPaths(BattleConstValue.BuffFolder, out var buffPaths))
			{
				_paths.paths.Add(EPathType.Buff,buffPaths);
			}
			else {
				Debug.LogError("Buff收集失败");
			}
			
			if (TryGetAllAssetPaths(BattleConstValue.BulletFolder, out var bulletsPaths))
			{
				_paths.paths.Add(EPathType.Bullet,bulletsPaths);
			}
			else {
				Debug.LogError("Bullet收集失败");
			}
			
			EditorUtility.SetDirty(_paths);
			AssetDatabase.SaveAssets();
		}
		
		private bool TryGetAllAssetPaths(string rootDirectory, out List<string> paths)
		{
			paths = new List<string>();
			// 检查目录是否存在
			if (!Directory.Exists(rootDirectory))
			{
				Debug.LogError("Directory does not exist: " + rootDirectory);
				return false;
			}

			try
			{
				// 获取所有 .asset 文件的路径，包括所有子目录
				string[] assetFiles = Directory.GetFiles(rootDirectory, "*.asset", SearchOption.AllDirectories);
				paths.AddRange(assetFiles);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error accessing directory: " + rootDirectory + "\n" + ex.Message);
				return false;
			}

			return true;
		}
		
	}
}