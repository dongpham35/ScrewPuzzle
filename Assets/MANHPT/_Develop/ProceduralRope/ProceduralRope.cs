using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ProceduralRope : MonoBehaviour
{
    [SerializeField] private Transform[] _pins;
    [SerializeField] private float[]     _particleMass;
    [SerializeField] private float       _damping              = 0.99f;
    [SerializeField] private float       _particleDistance     = 1;
    [SerializeField] private int         _constraintIterations = 50;
    [SerializeField] private bool        _pinTail;

    private Transform _head;
    private Transform _tail;
    private int       _particleCount;

    private UnsafeList<PhysicHelper.Particle> _particles;
    private TransformAccessArray              _transformAccessArray;
    private JobHandle                         _jobHandle;

    private void Start()
    {
        _head = _pins[0];
        _tail = _pins[^1];

        _particleCount = _pins.Length;

        _particles = new UnsafeList<PhysicHelper.Particle>(_particleCount, Allocator.Persistent);
        for (var i = 0; i < _particleCount; i++)
        {
            var t = i / (float)_particleCount;

            _particles.Add(new PhysicHelper.Particle
            {
                position     = Vector3.Lerp(_head.position, _tail.position, t),
                prevPosition = Vector3.Lerp(_head.position, _tail.position, t),
                mass         = _particleMass[i],
                isPinned     = i == 0 || (i == _particleCount - 1 && _pinTail)
            });
        }

        _transformAccessArray = new TransformAccessArray(_pins);
    }

    public void TogglePinTail()
    {
        _pinTail = !_pinTail;
        UpdatePinTail(_pinTail);
    }

    private void UpdatePinTail(bool pin)
    {
        var particle = _particles[_particleCount - 1];
        particle.isPinned              = pin;
        _particles[_particleCount - 1] = particle;
    }

    private void Update()
    {
        var integrationJob = new PhysicHelper.VerletIntegration
        {
            Particles = _particles,
            DeltaTime = Time.deltaTime,
            Damping   = _damping
        };

        var movePinsJob = new PhysicHelper.MoveParticles
        {
            Particles = _particles
        };

        var constraintJob = new JakobsenConstraintJob
        {
            Iterations       = _constraintIterations,
            ParticleDistance = _particleDistance,
            Particles        = _particles
        };

        _jobHandle = integrationJob.Schedule(_particleCount, 64);
        _jobHandle.Complete();

        _jobHandle = constraintJob.Schedule();
        _jobHandle.Complete();

        _jobHandle = movePinsJob.Schedule(_transformAccessArray);
        _jobHandle.Complete();

#if UNITY_EDITOR
        UpdatePinTail(_pinTail);
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_particles.Length == 0) return;

        for (var i = 0; i < _particleCount - 1; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_particles[i].position, _particles[i + 1].position);
        }
    }
#endif

    private void OnDestroy()
    {
        if (_particles.IsCreated) _particles.Dispose();
        if (_transformAccessArray.isCreated) _transformAccessArray.Dispose();
    }

    [BurstCompile]
    private struct JakobsenConstraintJob : IJob
    {
        [NativeDisableParallelForRestriction] public UnsafeList<PhysicHelper.Particle> Particles;

        public float ParticleDistance;
        public int   Iterations;


        public void Execute()
        {
            for (var n = 0; n < Iterations; n++)
            {
                JakobsenConstraint();
            }
        }

        private void JakobsenConstraint()
        {
            for (var i = 0; i < Particles.Length - 1; i++)
            {
                var p0 = Particles[i];
                var p1 = Particles[i + 1];

                var delta  = p1.position - p0.position;
                var length = delta.magnitude;
                var diff   = (length - ParticleDistance) / length;

                if (!p0.isPinned) p0.position += delta * diff / 2;
                if (!p1.isPinned) p1.position -= delta * diff / 2;

                Particles[i]     = p0;
                Particles[i + 1] = p1;
            }
        }
    }
}