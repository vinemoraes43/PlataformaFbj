import { Header } from '@/components/Header'
import { RatingStars } from '@/components/RatingStars'

export default function GameDetailsPage({ params }: { params: { id: string } }) {
    // Em uma aplicação real, esses dados viriam de uma API
    const game = {
        id: 1,
        title: "Elden Ring",
        developer: "FromSoftware",
        publisher: "Bandai Namco",
        releaseDate: "25 de fevereiro de 2022",
        genres: ["RPG", "Ação", "Mundo Aberto"],
        platforms: ["PC", "PlayStation 5", "Xbox Series X/S"],
        rating: 4.9,
        description: "Elden Ring é um jogo de RPG de ação desenvolvido pela FromSoftware e publicado pela Bandai Namco Entertainment. O jogo é um colaboração entre o diretor Hidetaka Miyazaki e o romancista de fantasia George R. R. Martin.",
        imageUrl: "/images/elden-ring.jpg"
    }

    const reviews = [
        {
            id: 1,
            user: "João Silva",
            rating: 5,
            date: "15/03/2023",
            comment: "Simplesmente espetacular! O mundo aberto mais bem feito que já vi, com uma liberdade incrível e desafios que recompensam a exploração."
        },
        {
            id: 2,
            user: "Maria Oliveira",
            rating: 4,
            date: "10/03/2023",
            comment: "Excelente jogo, mas extremamente difícil para iniciantes. A história é profunda, mas um pouco confusa em alguns momentos."
        }
    ]

    return (
        <div>
            <Header />
            
            <div className="container mx-auto px-4 py-8">
                {/* Banner do Jogo */}
                <div 
                    className="h-96 rounded-xl mb-8 bg-cover bg-center flex items-end"
                    style={{ backgroundImage: `url(${game.imageUrl})` }}
                >
                    <div className="bg-gradient-to-t from-black to-transparent w-full p-8 rounded-b-xl">
                        <h1 className="text-4xl font-bold text-white">{game.title}</h1>
                        <div className="flex items-center mt-2">
                            <RatingStars rating={game.rating} size="lg" />
                            <span className="ml-2 text-white text-lg">({game.rating})</span>
                        </div>
                    </div>
                </div>
                
                {/* Informações do Jogo */}
                <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                    <div className="lg:col-span-2">
                        <h2 className="text-2xl font-bold mb-4">Sobre o Jogo</h2>
                        <p className="text-gray-300 mb-6">{game.description}</p>
                        
                        <h2 className="text-2xl font-bold mb-4">Avaliações</h2>
                        
                        <div className="space-y-6">
                            {reviews.map(review => (
                                <div key={review.id} className="bg-gray-800 p-6 rounded-xl">
                                    <div className="flex justify-between items-center mb-4">
                                        <div>
                                            <h3 className="font-bold text-lg">{review.user}</h3>
                                            <p className="text-gray-400 text-sm">{review.date}</p>
                                        </div>
                                        <RatingStars rating={review.rating} />
                                    </div>
                                    <p>{review.comment}</p>
                                </div>
                            ))}
                        </div>
                        
                        <div className="mt-6">
                            <h3 className="text-xl font-bold mb-4">Deixe sua avaliação</h3>
                            <form className="space-y-4">
                                <div>
                                    <label className="block mb-2">Sua avaliação</label>
                                    <div className="flex">
                                        {[1, 2, 3, 4, 5].map(star => (
                                            <button key={star} type="button" className="text-2xl mr-1">
                                                ☆
                                            </button>
                                        ))}
                                    </div>
                                </div>
                                
                                <div>
                                    <label className="block mb-2">Comentário</label>
                                    <textarea 
                                        className="w-full bg-gray-800 text-white p-4 rounded-lg"
                                        rows={4}
                                        placeholder="O que você achou deste jogo?"
                                    />
                                </div>
                                
                                <button 
                                    type="submit"
                                    className="bg-yellow-500 hover:bg-yellow-600 text-gray-900 font-bold py-3 px-6 rounded-lg"
                                >
                                    Enviar Avaliação
                                </button>
                            </form>
                        </div>
                    </div>
                    
                    <div>
                        <div className="bg-gray-800 p-6 rounded-xl sticky top-24">
                            <h2 className="text-2xl font-bold mb-4">Detalhes</h2>
                            
                            <div className="space-y-4">
                                <div>
                                    <h3 className="font-bold">Desenvolvedora</h3>
                                    <p>{game.developer}</p>
                                </div>
                                
                                <div>
                                    <h3 className="font-bold">Publicadora</h3>
                                    <p>{game.publisher}</p>
                                </div>
                                
                                <div>
                                    <h3 className="font-bold">Data de Lançamento</h3>
                                    <p>{game.releaseDate}</p>
                                </div>
                                
                                <div>
                                    <h3 className="font-bold">Gêneros</h3>
                                    <div className="flex flex-wrap gap-2 mt-2">
                                        {game.genres.map(genre => (
                                            <span 
                                                key={genre}
                                                className="bg-gray-700 px-3 py-1 rounded-full text-sm"
                                            >
                                                {genre}
                                            </span>
                                        ))}
                                    </div>
                                </div>
                                
                                <div>
                                    <h3 className="font-bold">Plataformas</h3>
                                    <div className="flex flex-wrap gap-2 mt-2">
                                        {game.platforms.map(platform => (
                                            <span 
                                                key={platform}
                                                className="bg-purple-600 px-3 py-1 rounded-full text-sm"
                                            >
                                                {platform}
                                            </span>
                                        ))}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}