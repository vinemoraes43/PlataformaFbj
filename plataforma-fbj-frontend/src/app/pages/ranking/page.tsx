import { Header } from '@/components/Header'
import { RatingStars } from '@/components/RatingStars'

const topGames = [
    {
        id: 1,
        title: "Elden Ring",
        rating: 4.9,
        reviews: 1245
    },
    {
        id: 2,
        title: "The Legend of Zelda: Tears of the Kingdom",
        rating: 4.8,
        reviews: 987
    },
    {
        id: 3,
        title: "God of War Ragnarök",
        rating: 4.7,
        reviews: 856
    },
    // ... mais jogos
]

export default function RankingPage() {
    return (
        <div>
            <Header />
            
            <div className="container mx-auto px-4 py-8">
                <h1 className="text-3xl font-bold mb-8">Top Jogos do Ano</h1>
                
                <div className="bg-gray-800 rounded-xl overflow-hidden">
                    <table className="min-w-full">
                        <thead className="bg-gray-700">
                            <tr>
                                <th className="py-4 px-6 text-left">Posição</th>
                                <th className="py-4 px-6 text-left">Jogo</th>
                                <th className="py-4 px-6 text-left">Avaliação</th>
                                <th className="py-4 px-6 text-left">Avaliações</th>
                            </tr>
                        </thead>
                        <tbody>
                            {topGames.map((game, index) => (
                                <tr key={game.id} className="border-b border-gray-700 hover:bg-gray-750">
                                    <td className="py-4 px-6">
                                        <div className="text-2xl font-bold">
                                            {index === 0 ? "🥇" : index === 1 ? "🥈" : index === 2 ? "🥉" : `#${index + 1}`}
                                        </div>
                                    </td>
                                    <td className="py-4 px-6 font-bold">{game.title}</td>
                                    <td className="py-4 px-6">
                                        <RatingStars rating={game.rating} size="lg" />
                                    </td>
                                    <td className="py-4 px-6">{game.reviews.toLocaleString()}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
                
                <div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div className="bg-gray-800 p-6 rounded-xl">
                        <h2 className="text-xl font-bold mb-4">Melhores de Ação</h2>
                        <ul className="space-y-3">
                            {topGames.slice(0, 3).map((game, index) => (
                                <li key={game.id} className="flex justify-between items-center">
                                    <span>{index + 1}. {game.title}</span>
                                    <RatingStars rating={game.rating} size="sm" />
                                </li>
                            ))}
                        </ul>
                    </div>
                    
                    <div className="bg-gray-800 p-6 rounded-xl">
                        <h2 className="text-xl font-bold mb-4">Melhores RPGs</h2>
                        <ul className="space-y-3">
                            {topGames.slice(1, 4).map((game, index) => (
                                <li key={game.id} className="flex justify-between items-center">
                                    <span>{index + 1}. {game.title}</span>
                                    <RatingStars rating={game.rating} size="sm" />
                                </li>
                            ))}
                        </ul>
                    </div>
                    
                    <div className="bg-gray-800 p-6 rounded-xl">
                        <h2 className="text-xl font-bold mb-4">Lançamentos Recentes</h2>
                        <ul className="space-y-3">
                            {topGames.slice(2, 5).map((game, index) => (
                                <li key={game.id} className="flex justify-between items-center">
                                    <span>{index + 1}. {game.title}</span>
                                    <RatingStars rating={game.rating} size="sm" />
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    )
}