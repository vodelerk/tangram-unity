using System;
using System.Linq;
using System.Collections.Generic;
using Mapzen.VectorData.Filters;
using Mapzen.VectorData;
using UnityEngine;

namespace Mapzen.Unity
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mapzen/Style")]
    public class FeatureStyle : ScriptableObject
    {
        [Serializable]
        public class LayerStyle
        {
            [SerializeField]
            private string layerName;

            public Material Material;
            public PolygonBuilder.Options PolygonBuilderOptions;
            public PolylineBuilder.Options PolylineBuilderOptions;
            public int Order;

            public string LayerName
            {
                get { return layerName; }
            }

            public LayerStyle(string layerName)
            {
                this.layerName = layerName;
                this.Order = 0;
            }

            public PolygonBuilder.Options GetPolygonOptions(Feature feature, float inverseTileScale)
            {
                var options = PolygonBuilderOptions;

                options.LayerOffset = Order * 0.01f;
                options.Material = this.Material;

                if (options.MaxHeight > 0.0f)
                {
                    options.MaxHeight *= inverseTileScale;
                }
                else
                {
                    object heightValue;
                    if (feature.TryGetProperty("height", out heightValue) && heightValue is double)
                    {
                        options.MaxHeight = (float)((double)heightValue * inverseTileScale);
                    }
                }

                return options;
            }

            public PolylineBuilder.Options GetPolylineOptions(Feature feature, float inverseTileScale)
            {
                var options = PolylineBuilderOptions;

                options.LayerOffset = Order * 0.01f;
                options.Material = this.Material;
                options.Width *= inverseTileScale;
                options.MaxHeight *= inverseTileScale;

                return options;
            }
        }

        [Serializable]
        public class FilterStyle
        {
            [SerializeField]
            private string name;

            [SerializeField]
            private List<LayerStyle> layerStyles;

            [SerializeField]
            private FeatureFilter filter;

            public List<LayerStyle> LayerStyles
            {
                get { return layerStyles; }
            }

            public FeatureFilter Filter
            {
                get { return filter; }
                set { filter = value; }
            }

            public string Name
            {
                get { return name; }
            }

            public FilterStyle(string name)
            {
                this.name = name;
                this.layerStyles = new List<LayerStyle>();
                this.filter = new FeatureFilter();
            }

            public void AddLayerStyle(LayerStyle layerStyle)
            {
                layerStyles.Add(layerStyle);
                filter.CollectionNameSet.Add(layerStyle.LayerName);
            }

            public void RemoveLayerStyle(LayerStyle layerStyle)
            {
                layerStyles.Remove(layerStyle);
                filter.CollectionNameSet.Remove(layerStyle.LayerName);
            }
        }

        [SerializeField]
        private List<FilterStyle> filterStyles;

        public List<FilterStyle> FilterStyles
        {
            get { return filterStyles; }
        }

        #if UNITY_EDITOR
        public object Editor;
        #endif

        public FeatureStyle()
        {
            this.filterStyles = new List<FilterStyle>();
        }

        public void AddFilterStyle(FilterStyle filterStyle)
        {
            filterStyles.Add(filterStyle);
        }

        public void RemoveFilterStyle(FilterStyle filterStyle)
        {
            filterStyles.Remove(filterStyle);
        }
    }
}
