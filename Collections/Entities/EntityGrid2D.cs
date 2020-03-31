using System;
using System.Collections.Generic;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using UnityUtilities.Collections.Generic;

namespace UnityUtilities {
    namespace Collections {
        namespace Entities {
            /// <summary>
            /// A list of Entities that are arranged in a 2D grid.
            /// Handles positioning and updating Entities in the grid when cell
            /// size or padding are updated.
            /// </summary>
            public class EntityGrid2D : List2D<Entity> {
                // TODO Switch columns and rows so everything is in rows/x, columns/y format. Easier to remember.

                /// <summary>
                /// How large each cell of the grid is in Unity units. Readonly.
                /// </summary>
                public float2 CellSize { get; private set; }

                public GridMode RenderMode { get; private set; }

                private const int _pixelsPerUnit = 100;

                private EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

                /// <summary>
                /// Creates a 1x1 grid with cell size and padding of 0x0.
                /// </summary>
                public EntityGrid2D(GridMode gridMode) : this(gridMode, 1, 1) { }

                /// <summary>
                /// Creates a grid with rows and columns. Cell size and padding of 0x0.
                /// </summary>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                public EntityGrid2D(GridMode gridMode, int rows, int cols) : this(gridMode, rows, cols, new float2(1, 1)) { }

                /// <summary>
                /// Creates a grid with rows, columns, and cell size. Padding of 0x0.
                /// </summary>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                /// <param name="cellSize"></param>
                public EntityGrid2D(GridMode gridMode, int rows, int cols, float2 cellSize) : this(gridMode, new List<Entity>(), rows, cols, cellSize) { }

                /// <summary>
                /// Creates a grid from a list with rows and columns. Cell size and padding of 0x0.
                /// </summary>
                /// <param name="initialList"></param>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                public EntityGrid2D(GridMode gridMode, IEnumerable<Entity> initialList, int rows, int cols) : this(gridMode, initialList, rows, cols, new float2(1, 1)) { }

                /// <summary>
                /// Creates a grid from a list with rows, columns, cell size, and padding.
                /// </summary>
                /// <param name="initialList"></param>
                /// <param name="rows"></param>
                /// <param name="cols"></param>
                /// <param name="cellSize"></param>
                public EntityGrid2D(GridMode gridMode, IEnumerable<Entity> initialList, int rows, int cols, float2 cellSize) : base(initialList, rows, cols) {
                    CellSize = cellSize;
                    RenderMode = gridMode;
                    UpdateGridItems();
                }

                /// <summary>
                /// Add an item to the grid and position it accordingly.
                /// </summary>
                /// <param name="item"></param>
                public new void Add(Entity item) {
                    base.Add(item);
                    ApplyUpdateComponent(item);
                }

                /// <summary>
                /// Remove an object from the grid;
                /// </summary>T
                /// <param name="item"></param>
                /// <returns></returns>
                public new bool Remove(Entity item) {
                    bool removed = base.Remove(item);
                    if (removed) {
                        UpdateGridItems();
                    }
                    return removed;
                }

                /// <summary>
                /// Set the size each cell in the grid should be. Position of Entities are updated.
                /// </summary>
                /// <param name="cellSize"></param>
                public void SetCellSize(float2 cellSize) {
                    CellSize = cellSize;
                    UpdateGridItems();
                }

                /// <summary>
                /// Iterates through all entities in grid to add a component that
                /// indicates an update;
                /// </summary>
                private void UpdateGridItems() {
                    foreach (Entity e in this) {
                        ApplyUpdateComponent(e);
                    }
                }

                /// <summary>
                /// Apply a component to indicate to system below that the
                /// entity needs to be updated.
                /// </summary>
                /// <param name="e"></param>
                private void ApplyUpdateComponent(Entity e) {
                    Tuple<int, int> targetIndex = IndexOf2D(e);
                    manager.AddComponentData(e, new EntityTileNeedsUpdate {
                        CellSize = CellSize,
                        Rows = _rows,
                        Columns = _columns,
                        EntityIndex2D = new int2(targetIndex.Item1, targetIndex.Item2),
                        RenderMode = RenderMode
                    });
                }
            }

            /// <summary>
            /// Component for entity that needs to be updated.
            /// </summary>
            public struct EntityTileNeedsUpdate : IComponentData {
                public float2 CellSize;
                public int Rows;
                public int Columns;
                public int2 EntityIndex2D;
                public GridMode RenderMode;
            }

            /// <summary>
            /// System that iterates through all entities with the EntityTileNeedsUpdate component.
            /// Entities with that component need to have their position updated.
            /// </summary>
            public class EntityGridUpdateSystem : JobComponentSystem {
                private EntityCommandBufferSystem bufferSystem;

                protected override void OnCreate() {
                    base.OnCreate();
                    bufferSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();
                }

                protected override JobHandle OnUpdate(JobHandle inputDeps) {

                    EntityCommandBuffer.Concurrent buffer = bufferSystem.CreateCommandBuffer().ToConcurrent();

                    return Entities.ForEach((Entity gridTile, int entityInQueryIndex, ref Translation translation, in EntityTileNeedsUpdate needsUpdate) => {
                        float z = translation.Value.z;
                        float x = (needsUpdate.EntityIndex2D.y * needsUpdate.CellSize.x) -
                            (needsUpdate.Rows * needsUpdate.CellSize.x / 2);
                        float y = (-needsUpdate.EntityIndex2D.x * needsUpdate.CellSize.y) -
                            (needsUpdate.Columns * needsUpdate.CellSize.y / 2);

                        if (needsUpdate.RenderMode == GridMode.Mode3D) {
                            z = -y;
                            y = translation.Value.y;
                        }

                        translation.Value = new float3(x, y, z);
                        buffer.RemoveComponent<EntityTileNeedsUpdate>(entityInQueryIndex, gridTile);
                    }).Schedule(inputDeps);
                }
            }
        }
    }
}