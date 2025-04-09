﻿namespace WebApi.api.Models
{
    public class Object2D
    {
        public Guid id { get; set; }
        public Guid EnvironmentId { get; set; }
        public string PrefabId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float RotationZ { get; set; }
        public int SortingLayer { get; set; }
    }
}
