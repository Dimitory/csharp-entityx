using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entityx
{
    class Program
    {
        static void Main(string[] args)
        {

            struct Explosion
        {
            explicit Explosion(int damage) : damage(damage) { }
            int damage;
        };


        struct Collision
        {
            explicit Collision(int damage) : damage(damage) { }
            int damage;
        };

        struct ExplosionSystem : public Receiver<ExplosionSystem> {
  void receive(const Explosion &explosion) {
    damage_received += explosion.damage;
    received_count++;
  }

    void receive(const Collision &collision)
    {
        damage_received += collision.damage;
        received_count++;
    }

    int received_count = 0;
    int damage_received = 0;
};
        }
    }
}
