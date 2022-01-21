using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RegularExpressions
{
	class Program
	{
		public const string INPUT_FILE = "../../../input.txt";
		public const string OUTPUT_FILE = "../../../output.txt";
		internal const string FINAL_STATE = "F";

		static void Main(string[] args)
		{

			List<string> strings = new List<string>();

			using (StreamReader streamReader = new StreamReader(INPUT_FILE))
			{
				String line;
				while ((line = streamReader.ReadLine()) != null)
				{
					strings.Add(line);
				}
			}
			Console.WriteLine(strings.First());
			switch (strings.First())
			{
				case "RG":
					RegularExpressionsConverter rightConverter = new RightGrammarRegularExpressionsConverter();
					strings.Remove("RG");
					rightConverter.GetExpressions(strings);
					Dictionary<string, List<StateToTransition>> rigthExpressions = rightConverter.DeleteZeroTransitions();
					rightConverter.WriteExpressions(rigthExpressions);
					break;
				case "LG":
					RegularExpressionsConverter leftConverter = new LeftGrammarRegularExpressionsConverter();
					strings.Remove("LG");
					leftConverter.GetExpressions(strings);
					Dictionary<string, List<StateToTransition>> leftExpressions = leftConverter.DeleteZeroTransitions();
					leftConverter.WriteExpressions(leftExpressions);
					break;
				default:
					throw new ArgumentOutOfRangeException("Invalid grammar type");
			}

			/*Dictionary<string, Dictionary<string, string>> transitions = new Dictionary<string, Dictionary<string, string>>();

			foreach (string s in strings)
			{
				string state = s.Split(" -> ").First();
				List<string> statesToTransition = s.Split(" -> ")[1].Split(" | ").ToList();
				Dictionary<string, string> statesAndTransitions = new Dictionary<string, string>();
				string toState = String.Empty;
				foreach (string transitionAndState in statesToTransition)
				{
					toState = transitionAndState.Length == 1 ? FINAL_STATE : transitionAndState[1].ToString();
					statesAndTransitions.Add(toState, transitionAndState[0].ToString());
				}

				transitions.Add(state, statesAndTransitions);
			}

			transitions.Add("F", new Dictionary<string, string>());

			Dictionary<string, Dictionary<string, string>> transitionsWithOutEpsilon = new Dictionary<string, Dictionary<string, string>>();
			Dictionary<string, Dictionary<string, string>> transitionsWithEpsilon = new Dictionary<string, Dictionary<string, string>>();

			foreach (KeyValuePair<string, Dictionary<string, string>> transition in transitions)
			{
				transitionsWithEpsilon.Add(transition.Key, new Dictionary<string, string>());
				List<string> toStatesByEpsilon = transition.Value.Where(x => x.Value == "e").Select(x => x.Key).ToList();

				foreach(string toState in toStatesByEpsilon)
				{
					transitionsWithEpsilon[transition.Key].Add(toState, "e");
				}

				while (toStatesByEpsilon.Any())
				{
					List<string> newToStatesByEpsilon = new List<string>();

					foreach(string toState in toStatesByEpsilon)
					{
						newToStatesByEpsilon.AddRange(transitions[toState].Where(t => t.Value == "e").Select(t => t.Key));
					}

					toStatesByEpsilon = newToStatesByEpsilon.Distinct().ToList();

					foreach (string toState in toStatesByEpsilon)
					{
						transitionsWithEpsilon[transition.Key].Add(toState, "e");
					}
				}

				transitionsWithOutEpsilon.Add(transition.Key, new Dictionary<string, string>());

				foreach (string state in transitionsWithEpsilon[transition.Key].Keys)
				{
					Dictionary<string, string> statesToTransitions = transitions[state].Where(x => x.Value != "e").ToDictionary(x => x.Key, x => x.Value);
					foreach(KeyValuePair<string, string> stateToTransition in statesToTransitions)
					{
						transitionsWithOutEpsilon[transition.Key].Add(stateToTransition.Key, stateToTransition.Value);
					}
				}

			}

			using (StreamWriter streamWriter = new StreamWriter(OUTPUT_FILE))
			{
				foreach (KeyValuePair<string, Dictionary<string, string>> transition in transitionsWithOutEpsilon)
				{
					if (transition.Key == FINAL_STATE)
					{
						continue;
					}
					streamWriter.Write($"{transition.Key} -> ");
					string statesToTransitions = "";
					foreach(KeyValuePair<string, string> stateToTransition in transition.Value)
					{
						string state = stateToTransition.Key == FINAL_STATE ? "" : stateToTransition.Key;
						statesToTransitions += $"{stateToTransition.Value}{state} | ";
					}

					foreach (KeyValuePair<string, string> stateToTransition in transitions[transition.Key])
					{
						string state = stateToTransition.Key == FINAL_STATE ? "" : stateToTransition.Key;
						if (stateToTransition.Value != "e")
						{
							statesToTransitions += $"{stateToTransition.Value}{state} | ";
						}
					}

					statesToTransitions = statesToTransitions.Substring(0, statesToTransitions.Length - 3);

					streamWriter.WriteLine(statesToTransitions);
				}
			}*/

		}
	}
}
