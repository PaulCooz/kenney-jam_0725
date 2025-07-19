using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JamSpace
{
    public sealed class FloorGrid : MonoBehaviour
    {
        [SerializeField]
        private MeshWithProb[] meshesWithProb;

        [SerializeField, HideInInspector]
        private List<MeshFilter> tiles;

        private void Awake()
        {
            var sum = meshesWithProb.Sum(mp => mp.prob);
            foreach (var tile in tiles)
            {
                var rand = Random.Range(0f, sum);
                var index = 0;
                for (var i = 0; i < meshesWithProb.Length; i++)
                {
                    rand -= meshesWithProb[i].prob;
                    if (rand <= 0f)
                    {
                        index = i;
                        break;
                    }
                }

                tile.mesh = meshesWithProb[index].mesh;
            }
        }

        [Serializable]
        private struct MeshWithProb
        {
            public Mesh mesh;
            public float prob;
        }
    }
}