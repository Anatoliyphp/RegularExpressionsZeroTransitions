using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RegularExpressions
{
	class LeftGrammarRegularExpressionsConverter: RegularExpressionsConverter
	{
		public override void GetExpressions(List<string> lines)
		{
			_transitions = new Dictionary<string, List<StateToTransition>>();

			foreach (string s in lines)
			{
				string state = s.Split(" -> ").First();
				List<string> statesToTransition = s.Split(" -> ")[1].Split(" | ").ToList();
				string fromState = String.Empty;
				foreach (string transitionAndState in statesToTransition)
				{
					fromState = transitionAndState.Length == 1 ? FINAL_STATE : transitionAndState[0].ToString();
					string transition = fromState == FINAL_STATE ? transitionAndState[0].ToString() : transitionAndState[1].ToString();
					StateToTransition stateToTransition = new StateToTransition() { Key = state, Value = transition };
					if (_transitions.ContainsKey(fromState))
					{
						_transitions[fromState].Add(stateToTransition);
					}
					else
					{
						_transitions.Add(fromState, new List<StateToTransition>() { { new StateToTransition() { Key = state, Value = transition } } });
					}
				}

				//_transitions.Add(fromState, statesAndTransitions);
			}
		}

		public override void WriteExpressions(Dictionary<string, List<StateToTransition>> transitionsWithOutEpsilon)
		{
			using (StreamWriter streamWriter = new StreamWriter(OUTPUT_FILE))
			{
				foreach (KeyValuePair<string, List<StateToTransition>> transition in transitionsWithOutEpsilon)
				{
					streamWriter.Write($"{transition.Key} -> ");
					string statesToTransitions = "";
					foreach (StateToTransition stateToTransition in transition.Value)
					{
						string state = stateToTransition.Key;
						statesToTransitions += $"{stateToTransition.Value}{state} | ";
					}

					foreach (StateToTransition stateToTransition in _transitions[transition.Key])
					{
						string state = stateToTransition.Key;
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
