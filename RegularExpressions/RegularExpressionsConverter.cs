using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegularExpressions
{
	public abstract class RegularExpressionsConverter
	{
		internal Dictionary<string, List<StateToTransition>> _transitions;
		public const string FINAL_STATE = "F";
		public const string OUTPUT_FILE = "../../../output.txt";
		public virtual void GetExpressions(List<string> lines)
		{
			throw new NotImplementedException();
		}

		public Dictionary<string, List<StateToTransition>> DeleteZeroTransitions()
		{
			Dictionary<string, List<StateToTransition>> transitionsWithOutEpsilon = new Dictionary<string, List<StateToTransition>>();
			Dictionary<string, Dictionary<string, string>> transitionsWithEpsilon = new Dictionary<string, Dictionary<string, string>>();

			foreach (KeyValuePair<string, List<StateToTransition>> transition in _transitions)
			{
				transitionsWithEpsilon.Add(transition.Key, new Dictionary<string, string>());
				List<string> toStatesByEpsilon = transition.Value.Where(x => x.Value == "e").Select(x => x.Key).ToList();

				foreach (string toState in toStatesByEpsilon)
				{
					transitionsWithEpsilon[transition.Key].Add(toState, "e");
				}

				while (toStatesByEpsilon.Any())
				{
					List<string> newToStatesByEpsilon = new List<string>();

					foreach (string toState in toStatesByEpsilon)
					{
						newToStatesByEpsilon.AddRange(_transitions[toState].Where(t => t.Value == "e").Select(t => t.Key));
					}

					toStatesByEpsilon = newToStatesByEpsilon.Distinct().ToList();

					foreach (string toState in toStatesByEpsilon)
					{
						transitionsWithEpsilon[transition.Key].Add(toState, "e");
					}
				}

				transitionsWithOutEpsilon.Add(transition.Key, new List<StateToTransition>());

				foreach (string state in transitionsWithEpsilon[transition.Key].Keys)
				{
					Dictionary<string, string> statesToTransitions = _transitions[state].Where(x => x.Value != "e").ToDictionary(x => x.Key, x => x.Value);
					foreach (KeyValuePair<string, string> stateToTransition in statesToTransitions)
					{
						transitionsWithOutEpsilon[transition.Key].Add(new StateToTransition() {Key = stateToTransition.Key, Value = stateToTransition.Value });
					}
				}
			}
			return transitionsWithOutEpsilon;
		}

		public virtual void WriteExpressions(Dictionary<string, List<StateToTransition>> transitionsWithOutEpsilon)
		{
			throw new NotImplementedException();
		}
	}
}
