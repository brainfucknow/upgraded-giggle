import { Link } from 'react-router';
import { useGetBlogPosts, useDeleteBlogPost } from '../hooks/useBlogPosts';
import type { BlogPost } from '../lib/types';

function BlogPostCard({ post }: { post: BlogPost }) {
  const deleteMutation = useDeleteBlogPost();

  const handleDelete = async (e: React.MouseEvent) => {
    e.preventDefault();
    if (confirm('Are you sure you want to delete this blog post?')) {
      deleteMutation.mutate(post.id);
    }
  };

  return (
    <div className="border rounded-lg p-6 shadow-sm hover:shadow-md transition-shadow">
      <h2 className="text-xl font-semibold mb-2">{post.title}</h2>
      <p className="text-gray-600 mb-4 overflow-hidden" style={{
        display: '-webkit-box',
        WebkitLineClamp: 3,
        WebkitBoxOrient: 'vertical'
      }}>{post.body}</p>
      <div className="flex justify-between items-center">
        <span className="text-sm text-gray-500">
          {new Date(post.createdAt).toLocaleDateString()}
        </span>
        <div className="flex gap-2">
          <Link
            to={`/posts/${post.id}`}
            className="px-3 py-1 text-sm bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
          >
            View
          </Link>
          <Link
            to={`/posts/${post.id}/edit`}
            className="px-3 py-1 text-sm bg-gray-500 text-white rounded hover:bg-gray-600 transition-colors"
          >
            Edit
          </Link>
          <button
            onClick={handleDelete}
            disabled={deleteMutation.isPending}
            className="px-3 py-1 text-sm bg-red-500 text-white rounded hover:bg-red-600 transition-colors disabled:opacity-50"
          >
            {deleteMutation.isPending ? 'Deleting...' : 'Delete'}
          </button>
        </div>
      </div>
    </div>
  );
}

export function BlogPostList() {
  const { data: posts, isLoading, error } = useGetBlogPosts();

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <div className="text-gray-600">Loading blog posts...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex justify-center items-center py-8">
        <div className="text-red-600">Error loading blog posts: {error.message}</div>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto p-6">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Blog Posts</h1>
        <Link
          to="/posts/new"
          className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition-colors"
        >
          Create New Post
        </Link>
      </div>
      
      {posts?.length === 0 ? (
        <div className="text-center py-8 text-gray-600">
          <p>No blog posts found.</p>
          <Link
            to="/posts/new"
            className="text-blue-500 hover:text-blue-600 underline"
          >
            Create your first post
          </Link>
        </div>
      ) : (
        <div className="grid gap-6">
          {posts?.map((post) => (
            <BlogPostCard key={post.id} post={post} />
          ))}
        </div>
      )}
    </div>
  );
}