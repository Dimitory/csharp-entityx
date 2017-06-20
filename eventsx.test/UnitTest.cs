using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eventsx.test
{
    struct Explosion
    {
        public Explosion(int damage)
        {
            this.damage = damage;
        }
        public int damage;
    };

    struct Collision
    {
        public Collision(int damage)
        {
            this.damage = damage;
        }
        public int damage;
    };

    class ExplosionSystem
    {
        public int received_count = 0;
        public int damage_received = 0;
        public void Receive(Explosion explosion)
        {
            damage_received += explosion.damage;
            received_count++;
        }

        public void Receive(Collision collision)
        {
            damage_received += collision.damage;
            received_count++;
        }
    };

    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestEmitReceive()
        {
            EventSystem es = new EventSystem();
            ExplosionSystem explosion_system = new ExplosionSystem();
            es.Subscribe<Explosion, ExplosionSystem>(explosion_system);
            es.Subscribe<Collision, ExplosionSystem>(explosion_system);
            Assert.AreEqual(0, explosion_system.damage_received);
            es.Emit<Explosion>(10);
            Assert.AreEqual(1, explosion_system.received_count);
            Assert.AreEqual(10, explosion_system.damage_received);
            es.Emit<Collision>(10);
            Assert.AreEqual(20, explosion_system.damage_received);
            Assert.AreEqual(2, explosion_system.received_count);
        }

        [TestMethod]
        public void TestUntypedEmitReceive()
        {
            EventSystem es = new EventSystem();
            ExplosionSystem explosion_system = new ExplosionSystem();
            es.Subscribe<Explosion, ExplosionSystem>(explosion_system);
            Assert.AreEqual(0, explosion_system.damage_received);
            Explosion explosion = new Explosion(10);
            es.Emit(explosion);
            Assert.AreEqual(1, explosion_system.received_count);
            Assert.AreEqual(10, explosion_system.damage_received);
        }

        [TestMethod]
        public void TestUnsubscription()
        {
            ExplosionSystem explosion_system = new ExplosionSystem();
            {
                EventSystem es = new EventSystem();
                es.Subscribe<Explosion, ExplosionSystem>(explosion_system);
                Assert.AreEqual(explosion_system.damage_received, 0);
                es.Emit<Explosion>(1);
                Assert.AreEqual(explosion_system.damage_received, 1);
                es.Unsubscribe<Explosion, ExplosionSystem>(explosion_system);
                es.Emit<Explosion>(1);
                Assert.AreEqual(explosion_system.damage_received, 1);
            }
        }
    }
}
