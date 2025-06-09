export const RatingStars = ({ rating, size = 'md' }: { 
    rating: number, 
    size?: 'sm' | 'md' | 'lg' 
}) => {
    const starSize = size === 'sm' ? 'text-lg' : size === 'lg' ? 'text-2xl' : 'text-xl'
    
    return (
        <div className="flex items-center">
            <div className={`${starSize} text-yellow-500 mr-1`}>
                {[...Array(5)].map((_, i) => (
                    <span key={i}>{i < Math.floor(rating) ? '★' : '☆'}</span>
                ))}
            </div>
            <span className="text-white font-bold">{rating.toFixed(1)}</span>
        </div>
    )
}