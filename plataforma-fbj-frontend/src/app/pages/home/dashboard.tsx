import { Header } from '@/components/Header'
import { RatingStars } from '@/components/RatingStars'

export default function DashboardPage() {
    const userReviews = [
        {
            id: 1,
            game: "Elden Ring",
            rating: 5,
            date: "15/03/2023"
        },
        {
            id: 2,
            game: "God of War Ragnarök",
            rating: 4,
            date: "10/02/2023"
        },
        // ... mais avaliações do usuário
    ]

    return (
        <div>
            <Header />
            
            <div className="container mx-auto px-4 py-8">
                <h1 className="text-3xl font-bold mb-8">Minha Conta</h1>
                
                <div className="flex flex-col md:flex-row gap-8">
                    <div className="md:w-1/3">
                        <div className="bg-gray-800 p-6 rounded-xl">
                            <div className="flex items-center mb-6">
                                <div className="bg-gray-200 border-2 border-dashed rounded-xl w-16 h-16" />
                                <div className="ml-4">
                                    <h2 className="text-xl font-bold">João Silva</h2>
                                    <p className="text-gray-400">Membro desde Jan 2022</p>
                                </div>
                            </div>
                            
                            <div className="space-y-4">
                                <div>
                                    <h3 className="font-bold">Total de Avaliações</h3>
                                    <p className="text-2xl">24</p>
                                </div>
                                
                                <div>
                                    <h3 className="font-bold">Média de Nota</h3>
                                    <div className="flex items-center">
                                        <RatingStars rating={4.2} />
                                        <span className="ml-2">4.2</span>
                                    </div>
                                </div>
                                
                                <div>
                                    <h3 className="font-bold">Jogo Mais Avaliado</h3>
                                    <p>Elden Ring (3 avaliações)</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div className="md:w-2/3">
                        <div className="bg-gray-800 p-6 rounded-xl mb-8">
                            <h2 className="text-xl font-bold mb-4">Minhas Últimas Avaliações</h2>
                            
                            <div className="space-y-4">
                                {userReviews.map(review => (
                                    <div key={review.id} className="flex justify-between items-center border-b border-gray-700 pb-4">
                                        <div>
                                            <h3 className="font-bold">{review.game}</h3>
                                            <p className="text-gray-400 text-sm">{review.date}</p>
                                        </div>
                                        <RatingStars rating={review.rating} />
                                    </div>
                                ))}
                            </div>
                            
                            <button className="mt-4 w-full bg-gray-700 hover:bg-gray-600 py-2 rounded-lg">
                                Ver Todas as Avaliações
                            </button>
                        </div>
                        
                        <div className="bg-gray-800 p-6 rounded-xl">
                            <h2 className="text-xl font-bold mb-4">Jogos que Quero Avaliar</h2>
                            
                            <div className="grid grid-cols-2 sm:grid-cols-3 gap-4">
                                {[1, 2, 3, 4, 5, 6].map(id => (
                                    <div key={id} className="bg-gray-700 rounded-lg p-4">
                                        <div className="bg-gray-600 w-full h-32 rounded mb-2" />
                                        <p className="font-bold">Jogo {id}</p>
                                        <button className="mt-2 text-sm bg-yellow-500 hover:bg-yellow-600 text-gray-900 py-1 px-2 rounded w-full">
                                            Avaliar
                                        </button>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}