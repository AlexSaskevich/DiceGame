using System;

namespace Source.ComponentContainer
{
    public interface IComponentContainer
    {
        public IComponentContainer AddComponent<T>(T component) where T : class;
        public T AddOrGetComponent<T>() where T : class, new();
        public bool TryGetComponent<T>(out T component) where T : class;
        public T GetComponent<T>() where T : class;
        public T GetComponent<T>(Type type);
        public void RemoveComponent<T>(T component) where T : class;
    }
}