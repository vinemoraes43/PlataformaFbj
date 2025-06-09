import { Header } from '@/components/Header'

const latestReviews = [
    {
        id: 1,
        game: "The Legend of Zelda: Tears of the Kingdom",
        user: "Carlos Santos",
        rating: 5,
        comment: "Uma obra-prima que supera todas as expectativas. A liberdade criativa com o sistema de construção é revolucionária.",
        date: "12/05/2023"
    },
    {
        id: 2,
        game: "Resident Evil 4 Remake",
        user: "Ana Pereira",
        rating: 4.5,
        comment: "Uma refilmagem que honra o original enquanto adiciona elementos modernos. O combate é intenso e satisfatório.",
        date: "05/05/2023"
    },
    // ... mais avaliações
]

export default function AvaliacoesPage() {
    return (
        <div>
            <Header />
            
            <div className="container mx-auto px-4 py-8">
                <h1 className="text-3xl font-bold mb-8">Últimas Avaliações</h1>
                
                <div className="space-y-6">
                    {latestReviews.map(review => (
                        <div key={review.id} className="bg-gray-800 p-6 rounded-xl">
                            <div className="flex justify-between items-center mb-4">
                                <div>
                                    <h2 className="text-xl font-bold">{review.game}</h2>
                                    <p className="text-gray-400">Por {review.user} em {review.date}</p>
                                </div>
                                <div className="text-2xl text-yellow-500">
                                    {'★'.repeat(review.rating)}{'☆'.repeat(5 - review.rating)}
                                </div>
                            </div>
                            
                            <p className="text-gray-300">{review.comment}</p>
                        </div>
                    ))}
                </div>
                
                <div className="mt-8 flex justify-center">
                    <button className="bg-gray-800 hover:bg-gray-700 text-white px-6 py-3 rounded">
                        Carregar Mais Avaliações
                    </button>
                </div>
            </div>
        </div>
    )
}