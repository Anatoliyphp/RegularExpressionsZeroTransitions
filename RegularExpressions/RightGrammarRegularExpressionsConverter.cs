using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RegularExpressions
{
	class RightGrammarRegularExpressionsConverter: RegularExpressionsConverter
	{
		public override void GetExpressions(List<string> lines)
		{
			_transitions = new Dictionary<string, List<StateToTransition>>();

			foreach (string s in lines)
			{
				string state = s.Split(" -> ").First();
				List<string> statesToTransition = s.Split(" -> ")[1].Split(" | ").ToList();
				List<StateToTransition> statesAndTransitions = new List<StateToTransition>();
				string toState = String.Empty;
				foreach (string transitionAndState in statesToTransition)
				{
					toState = transitionAndState.Length == 1 ? FINAL_STATE : transitionAndState[1].ToString();
					statesAndTransitions.Add(new StateToTransition() { Key = toState, Value = transitionAndState[0].ToString() });
				}

				_transitions.Add(state, statesAndTransitions);
			}

			_transitions.Add("F", new List<StateToTransition>());
		}

		public override void WriteExpressions(Dictionary<string, List<StateToTransition>> transitionsWithOutEpsilon)
		{
			using (StreamWriter streamWriter = new StreamWriter(OUTPUT_FILE))
			{
				foreach (KeyValuePair<string, List<StateToTransition>> transition in transitionsWithOutEpsilon)
				{
					if (transition.Key == FINAL_STATE)
					{
						continue;
					}
					streamWriter.Write($"{transition.Key} -> ");
					string statesToTransitions = "";
					foreach (StateToTransition stateToTransition in transition.Value)
					{
						string state = stateToTransition.Key == FINAL_STATE ? "" : stateToTransition.Key;
						statesToTransitions += $"{stateToTransition.Value}{state} | ";
					}

					foreach (StateToTransition stateToTransition in _transitions[transition.Key])
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
			}
		}
	}
}
