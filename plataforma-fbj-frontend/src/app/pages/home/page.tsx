import { Header } from '@/components/Header'
import { GameCard } from '@/components/GameCard'

const featuredGames = [
    {
        id: 1,
        title: "Elden Ring",
        genre: "RPG/Ação",
        releaseYear: 2022,
        rating: 4.9,
        imageUrl: "/images/elden-ring.jpg"
    },
    {
        id: 2,
        title: "God of War Ragnarök",
        genre: "Ação/Aventura",
        releaseYear: 2022,
        rating: 4.8,
        imageUrl: "/images/god-of-war.jpg"
    },
    {
        id: 3,
        title: "The Legend of Zelda: Tears of the Kingdom",
        genre: "Ação/Aventura",
        releaseYear: 2023,
        rating: 4.7,
        imageUrl: "/images/zelda.jpg"
    }
]

export default function HomePage() {
    return (
        <div>
            <Header />
            
            {/* Banner Hero */}
            <div className="bg-gradient-to-r from-purple-900 to-indigo-800 text-white py-16">
                <div className="container mx-auto px-4 text-center">
                    <h1 className="text-4xl md:text-6xl font-bold mb-4">Compartilhe sua paixão por jogos</h1>
                    <p className="text-xl mb-8 max-w-2xl mx-auto">
                        Descubra, avalie e compartilhe suas opiniões sobre os melhores jogos do mercado.
                    </p>
                    <button className="bg-yellow-500 hover:bg-yellow-600 text-gray-900 font-bold py-3 px-8 rounded-full text-lg">
                        Comece a Avaliar
                    </button>
                </div>
            </div>

            {/* Jogos em Destaque */}
            <section className="py-12 bg-gray-100">
                <div className="container mx-auto px-4">
                    <h2 className="text-3xl font-bold mb-8 text-center">Jogos em Destaque</h2>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                        {featuredGames.map(game => (
                            <GameCard key={game.id} game={game} />
                        ))}
                    </div>
                </div>
            </section>

            {/* Como Funciona */}
            <section className="py-12">
                <div className="container mx-auto px-4">
                    <h2 className="text-3xl font-bold mb-8 text-center">Como Funciona</h2>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                        <div className="text-center p-6 bg-white rounded-xl shadow">
                            <div className="text-4xl mb-4">1</div>
                            <h3 className="text-xl font-bold mb-2">Encontre Jogos</h3>
                            <p>Busque na nossa vasta biblioteca de jogos</p>
                        </div>
                        <div className="text-center p-6 bg-white rounded-xl shadow">
                            <div className="text-4xl mb-4">2</div>
                            <h3 className="text-xl font-bold mb-2">Avalie</h3>
                            <p>Dê sua nota e escreva sua análise</p>
                        </div>
                        <div className="text-center p-6 bg-white rounded-xl shadow">
                            <div className="text-4xl mb-4">3</div>
                            <h3 className="text-xl font-bold mb-2">Compartilhe</h3>
                            <p>Discuta com outros jogadores apaixonados</p>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    )
}