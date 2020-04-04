using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Systems;

namespace UnityUtilities {
    namespace Physics {
        namespace Entities {

            /// <summary>
            /// Creates an Entity raycaster by getting BuildPhysicsWorld
            /// and CollisionWorld to perform the casts.
            /// </summary>
            public class Raycaster {
                private BuildPhysicsWorld _buildPhysicsWorld;
                private CollisionWorld _collisionWorld;

                public Raycaster() {
                    _buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
                    _collisionWorld = _buildPhysicsWorld.PhysicsWorld.CollisionWorld;
                }

                /// <summary>
                /// Perform raycast from one position to another. And returns
                /// first Entity hit by the ray.
                /// </summary>
                /// <param name="from">Origin of ray.</param>
                /// <param name="to">Length/distance of ray.</param>
                /// <returns></returns>
                public Entity Raycast(float3 from, float3 to) {
                    RaycastHit hit = new RaycastHit();
                    RaycastInput input = new RaycastInput {
                        Start = from,
                        End = to,
                        Filter = new CollisionFilter {
                            BelongsTo = ~0u,
                            CollidesWith = ~0u,
                            GroupIndex = 0
                        }
                    };

                    if(_collisionWorld.CastRay(input, out hit)) {
                        return _buildPhysicsWorld.PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                    } else {
                        return default;
                    }
                }
            }
        }
    }
}