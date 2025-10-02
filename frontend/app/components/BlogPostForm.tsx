import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useCreateBlogPost, useUpdateBlogPost } from '../hooks/useBlogPosts';
import type { BlogPost, CreateBlogPostDto, UpdateBlogPostDto } from '../lib/types';

interface BlogPostFormProps {
  post?: BlogPost;
  isEdit?: boolean;
}

export function BlogPostForm({ post, isEdit = false }: BlogPostFormProps) {
  const navigate = useNavigate();
  const createMutation = useCreateBlogPost();
  const updateMutation = useUpdateBlogPost();
  
  const [title, setTitle] = useState(post?.title || '');
  const [body, setBody] = useState(post?.body || '');

  useEffect(() => {
    if (post) {
      setTitle(post.title);
      setBody(post.body);
    }
  }, [post]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const data = { title, body };
    
    try {
      if (isEdit && post) {
        await updateMutation.mutateAsync({ id: post.id, data: data as UpdateBlogPostDto });
      } else {
        await createMutation.mutateAsync(data as CreateBlogPostDto);
      }
      navigate('/');
    } catch (error) {
      console.error('Error saving blog post:', error);
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  return (
    <div className="max-w-2xl mx-auto p-6">
      <h1 className="text-3xl font-bold mb-8">
        {isEdit ? 'Edit Post' : 'Create New Post'}
      </h1>
      
      <form onSubmit={handleSubmit} className="space-y-6">
        <div>
          <label htmlFor="title" className="block text-sm font-medium text-gray-700 mb-2">
            Title
          </label>
          <input
            type="text"
            id="title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter post title..."
          />
        </div>

        <div>
          <label htmlFor="body" className="block text-sm font-medium text-gray-700 mb-2">
            Content
          </label>
          <textarea
            id="body"
            value={body}
            onChange={(e) => setBody(e.target.value)}
            required
            rows={12}
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-vertical"
            placeholder="Write your blog post content..."
          />
        </div>

        <div className="flex gap-4">
          <button
            type="submit"
            disabled={isLoading}
            className="px-6 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors disabled:opacity-50"
          >
            {isLoading ? 'Saving...' : isEdit ? 'Update Post' : 'Create Post'}
          </button>
          
          <button
            type="button"
            onClick={() => navigate('/')}
            className="px-6 py-2 bg-gray-300 text-gray-700 rounded hover:bg-gray-400 transition-colors"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
}