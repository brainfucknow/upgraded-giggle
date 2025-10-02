import { Link, useParams, useNavigate } from 'react-router';
import { useGetBlogPost, useDeleteBlogPost } from '../hooks/useBlogPosts';

export function BlogPostView() {
  const { id } = useParams();
  const navigate = useNavigate();
  const postId = id ? parseInt(id, 10) : 0;
  const { data: post, isLoading, error } = useGetBlogPost(postId);
  const deleteMutation = useDeleteBlogPost();

  const handleDelete = async () => {
    if (confirm('Are you sure you want to delete this blog post?')) {
      try {
        await deleteMutation.mutateAsync(postId);
        navigate('/');
      } catch (error) {
        console.error('Error deleting blog post:', error);
      }
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <div className="text-gray-600">Loading blog post...</div>
      </div>
    );
  }

  if (error || !post) {
    return (
      <div className="max-w-4xl mx-auto p-6">
        <div className="text-red-600 mb-4">
          {error?.message || 'Blog post not found'}
        </div>
        <Link
          to="/"
          className="text-blue-500 hover:text-blue-600 underline"
        >
          ← Back to all posts
        </Link>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto p-6">
      <div className="mb-6">
        <Link
          to="/"
          className="text-blue-500 hover:text-blue-600 underline"
        >
          ← Back to all posts
        </Link>
      </div>

      <article className="bg-white rounded-lg shadow-sm border p-8">
        <header className="mb-6">
          <h1 className="text-4xl font-bold text-gray-900 mb-2">{post.title}</h1>
          <p className="text-gray-600">
            Published on {new Date(post.createdAt).toLocaleDateString('en-US', {
              year: 'numeric',
              month: 'long',
              day: 'numeric'
            })}
          </p>
        </header>

        <div className="prose prose-lg max-w-none">
          {post.body.split('\n').map((paragraph, index) => (
            <p key={index} className="mb-4 text-gray-800 leading-relaxed">
              {paragraph}
            </p>
          ))}
        </div>

        <footer className="mt-8 pt-6 border-t border-gray-200">
          <div className="flex gap-4">
            <Link
              to={`/posts/${post.id}/edit`}
              className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
            >
              Edit Post
            </Link>
            <button
              onClick={handleDelete}
              disabled={deleteMutation.isPending}
              className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600 transition-colors disabled:opacity-50"
            >
              {deleteMutation.isPending ? 'Deleting...' : 'Delete Post'}
            </button>
          </div>
        </footer>
      </article>
    </div>
  );
}