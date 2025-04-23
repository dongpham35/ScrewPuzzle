using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public static class PhysicHelper
{
    public struct Constraint
    {
        public int   particleIndex0;
        public int   particleIndex1;
        public float distance;
    }

    public unsafe struct Particle
    {
        public Vector3   position;
        public Vector3   prevPosition;
        public float     mass;
        public bool      isPinned;
        public Particle* relatedParticles;
        public float*    distances;
        public int       relatedParticleCount;
    }

    public static float DistanceToRay(Particle particle, Ray ray)
    {
        var ab = particle.position - ray.origin;
        var t  = Vector3.Dot(ab, ray.direction);
        if (t <= 0) return Vector3.Distance(particle.position, ray.origin);
        var e = ray.origin + ray.direction * t;
        return Vector3.Distance(particle.position, e);
    }

    [BurstCompile]
    public struct FindClosestNotPinnedParticleToRay : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public UnsafeList<Particle>   Particles;
        [NativeDisableParallelForRestriction] public NativeReference<float> ClosestDistance;
        [NativeDisableParallelForRestriction] public NativeReference<int>   ClosestParticleIndex;

        public Ray Ray;

        public void Execute(int index)
        {
            var particle = Particles[index];
            if (particle.isPinned) return;
            var distance = DistanceToRay(particle, Ray);
            if (distance < ClosestDistance.Value)
            {
                ClosestDistance.Value      = distance;
                ClosestParticleIndex.Value = index;
            }
        }
    }

    [BurstCompile]
    public struct GetVertexFromParticle : IJobParallelFor
    {
        public UnsafeList<Particle> Particles;
        public NativeArray<Vector3> Vertices;

        public void Execute(int index)
        {
            Vertices[index] = Particles[index].position;
        }
    }

    [BurstCompile]
    public struct VerletIntegration : IJobParallelFor
    {
        public UnsafeList<Particle> Particles;
        public float                DeltaTime;
        public float                Damping;

        public void Execute(int index)
        {
            var particle = Particles[index];
            if (particle.isPinned) return;
            var acceleration = GetAcceleration(particle);
            var temp         = particle.position;
            particle.position = particle.position + (particle.position - particle.prevPosition) * Damping +
                                acceleration                                                    * DeltaTime * DeltaTime;
            particle.prevPosition = temp;
            Particles[index]      = particle;
        }

        private Vector3 GetAcceleration(PhysicHelper.Particle particle)
        {
            return Vector3.down * 9.8f * particle.mass;
        }
    }

    [BurstCompile]
    public struct MoveParticles : IJobParallelForTransform
    {
        public UnsafeList<Particle> Particles;

        public void Execute(int index, TransformAccess transform)
        {
            var particle = Particles[index];
            if (particle.isPinned)
            {
                particle.prevPosition = particle.position;
                particle.position     = transform.position;
            }
            else
            {
                transform.position = particle.position;
            }

            Particles[index] = particle;
        }
    }
}