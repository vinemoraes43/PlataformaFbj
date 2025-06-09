import { Header } from '@/components/Header'
import { GameCard } from '@/components/GameCard'
import { RatingStars } from '@/components/RatingStars'

const allGames = [
    // ... lista completa de jogos (20+ itens)
    {
        id: 4,
        title: "Cyberpunk 2077",
        genre: "RPG/Ação",
        releaseYear: 2020,
        rating: 3.8,
        imageUrl: "/images/cyberpunk.jpg"
    },
    {
        id: 5,
        title: "Horizon Forbidden West",
        genre: "Ação/Aventura",
        releaseYear: 2022,
        rating: 4.5,
        imageUrl: "/images/horizon.jpg"
    },
    // ... mais jogos
]

export default function JogosPage() {
    return (
        <div>
            <Header />
            
            <div className="container mx-auto px-4 py-8">
                <div className="flex justify-between items-center mb-8">
                    <h1 className="text-3xl font-bold">Todos os Jogos</h1>
                    
                    <div className="flex gap-4">
                        <select className="bg-gray-800 text-white px-4 py-2 rounded">
                            <option>Ordenar por: Mais Populares</option>
                            <option>Melhores Avaliados</option>
                            <option>Lançamentos Recentes</option>
                        </select>
                        
                        <select className="bg-gray-800 text-white px-4 py-2 rounded">
                            <option>Todos os Gêneros</option>
                            <option>Ação</option>
                            <option>Aventura</option>
                            <option>RPG</option>
                            <option>FPS</option>
                            <option>Esportes</option>
                        </select>
                    </div>
                </div>
                
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
                    {allGames.map(game => (
                        <GameCard key={game.id} game={game} />
                    ))}
                </div>
                
                <div className="mt-8 flex justify-center">
                    <button className="bg-gray-800 hover:bg-gray-700 text-white px-6 py-3 rounded">
                        Carregar Mais Jogos
                    </button>
                </div>
            </div>
        </div>
    )
}