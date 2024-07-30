namespace ET
{
    public interface ISingletonAwake
    {
        void Awake();
    }
    
    public interface ISingletonAwake<in A>
    {
        void Awake(A a);
    }
    
    public interface ISingletonAwake<in A, in B>
    {
        void Awake(A a, B b);
    }
    
    public interface ISingletonAwake<in A, in B, in C>
    {
        void Awake(A a, B b, C c);
    }
}