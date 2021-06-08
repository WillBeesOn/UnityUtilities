using System.Linq;
using System.Collections.Generic;

namespace UnityUtilities.Collections.Generic {
	public class MultiLinkedList<T> : List<MultiLinkedListNode<T>> {
		private readonly int _maxAdjacentNodes;

		public MultiLinkedList(IEnumerable<MultiLinkedListNode<T>> initialList, int numberOfAdjacentNodes) : base(initialList) {
			_maxAdjacentNodes = numberOfAdjacentNodes;
		}

		public MultiLinkedList(IEnumerable<T> initialList, int numberOfAdjacentNodes) {
			_maxAdjacentNodes = numberOfAdjacentNodes;

			foreach (var data in initialList) {
				Add(data);
			}
		}

		public void Add(T data) {
			Add(new MultiLinkedListNode<T>(data, this, _maxAdjacentNodes));
		}
	}

	public class MultiLinkedListNode<T> {
		public T data;
		private readonly MultiLinkedList<T> _parentList;
		private readonly List<int> _adjacentNodes;
		private readonly int _maxAdjacentNodes;

		public MultiLinkedListNode(T data, MultiLinkedList<T> parentList, int numberOfAdjacentNodes) {
			this.data = data;
			_adjacentNodes = new List<int>();
			_maxAdjacentNodes = numberOfAdjacentNodes;
			_parentList = parentList;
		}

		public List<MultiLinkedListNode<T>> GetAdjacentNodes() {
			return (from node in _parentList
			        where _adjacentNodes.Contains(_parentList.IndexOf(node))
			        select node).ToList();
		}

		public bool LinkNode(MultiLinkedListNode<T> node) {
			if (_adjacentNodes.Count == _maxAdjacentNodes) {
				return false;
			}

			if (!_parentList.Contains(node)) {
				_parentList.Add(node);
			}

			_adjacentNodes.Add(_parentList.IndexOf(node));
			node.LinkNode(this);
			return true;
		}

		public bool LinkNodes(ICollection<MultiLinkedListNode<T>> nodes) {
			if (nodes.Count > _maxAdjacentNodes &&
			    nodes.Count > _maxAdjacentNodes - _adjacentNodes.Count) {
				return false;
			}

			foreach (var node in nodes) {
				LinkNode(node);
			}

			return true;
		}

		public bool LinkNode(int nodeIndex) {
			return LinkNode(_parentList[nodeIndex]);
		}

		public bool LinkNodes(ICollection<int> nodeIndices) {
			return LinkNodes(
				(from node in _parentList
				where nodeIndices.Contains(_parentList.IndexOf(node))
				select node).ToList()
			);
		}

		public bool UnlinkNode(MultiLinkedListNode<T> node) {
			var nodeIndex = _parentList.IndexOf(node);
			if (!_adjacentNodes.Contains(nodeIndex)) {
				return false;
			}

			_adjacentNodes.Remove(nodeIndex);
			return true;
		}

		public bool UnlinkNodes(ICollection<MultiLinkedListNode<T>> nodes) {
			if (nodes.Count > _adjacentNodes.Count) {
				return false;
			}

			foreach (var node in nodes) {
				UnlinkNode(node);
			}

			return true;
		}
	}
}