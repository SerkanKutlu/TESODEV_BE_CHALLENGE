using System.Collections.Generic;
using System.Threading.Tasks;
using OrderApplication.Interfaces.Handlers;
using OrderDomain.Entities;

namespace OrderInfrastructure.Handlers
{
    //Observer Desing Pattern Subject
    public class Subject : ISubject
    {
        public string ProductId { get; set; }
        private readonly List<IObserver> _observers;

        public Subject(string productId)
        {
            ProductId = productId;
            _observers = new List<IObserver>();
        }
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task Notify()
        {
            foreach (var observer in _observers)
            {
                await observer.Update(this);
            }
        }

        public void SomeBusinessLogic()
        {
            //Burda bunun yerine notify() methodu
            //customer silindiği method içerisinde
            //çağrılabilir.
            
            //Mantık: Her diğer tarafta işlem yapılması 
            //Gerektiğinde Notify ile observer'a 
            //Mesaj gönderilip orda http isteği yapılması
            //planlanabilir.
        }
    }
}